﻿using System.Text;
using Markdown.MarkdownTags;
using Markdown.MarkdownTagValidators;

namespace Markdown.Parsers;

public class MarkdownLineParser : IParser
{
    private readonly IValidator validator;

    public MarkdownLineParser(IValidator validator)
    {
        this.validator = validator;
    }

    public IEnumerable<MarkdownTag> ParseMarkdownTags(string markdownLineParagraph)
    {
        var parsedTags = new List<MarkdownTag>();
        var tagsPosition = SearchPositions(markdownLineParagraph).ToArray();
        var positionsParsedTags = new HashSet<int>();
        var parsedBoldTags = new List<(MarkdownTag openTag, MarkdownTag closeTag)>();
        var parsedItalicsTags = new List<(MarkdownTag openTag, MarkdownTag closeTag)>();

        for (var i = 0; i < tagsPosition.Length; i++)
        {
            var singleTag = SearchSingleTag(tagsPosition[i].TagType, tagsPosition[i].Position, markdownLineParagraph);

            if (singleTag != null)
            {
                parsedTags.Add(singleTag);
                continue;
            }

            var pairedTags = SearchPairedTags(
                i + 1,
                tagsPosition[i].TagType,
                tagsPosition[i].Position,
                positionsParsedTags,
                tagsPosition,
                markdownLineParagraph,
                parsedItalicsTags);
            
            if (pairedTags == null) 
                continue;
                
            if (tagsPosition[i].TagType == MarkdownTagType.Bold)
                parsedBoldTags.Add(pairedTags.Value);
            else
                parsedItalicsTags.Add(pairedTags.Value);
        }

        var disjointTags = GetDisjointTags(parsedItalicsTags, parsedBoldTags);
        parsedTags.AddRange(disjointTags);
        
        return parsedTags;
    }

    public IEnumerable<string> ParseMarkdownTextIntoParagraphs(string markdownText)
    {
        var lines = markdownText.Split('\n');
        var paragraph = new StringBuilder();

        foreach (var line in lines)
        {
            var isEmptyLine = string.IsNullOrWhiteSpace(line);

            if (!isEmptyLine && !IsHeadingLine(line))
            {
                paragraph.Append(line.Trim()).Append(' ');

                if (line.Length <= 1 || !line.EndsWith("  ")) 
                    continue;
            
                yield return paragraph.ToString().TrimEnd();
                paragraph.Clear();
            }
            if (paragraph.Length != 0)
                yield return paragraph.ToString().TrimEnd();
            if (IsHeadingLine(line))
                yield return line.Trim();
            paragraph.Clear();
        }
        
        if (paragraph.Length != 0) 
            yield return paragraph.ToString().TrimEnd();
    }

    private bool IsHeadingLine(string line)
    {
        var trimStartLine = line.TrimStart();

        return trimStartLine.Length > 0 && trimStartLine[0] == '#' &&
               validator.IsValidTag(MarkdownTagType.Heading, line.Length - trimStartLine.Length, line);
    }

    private MarkdownTag? SearchSingleTag(MarkdownTagType tagType, int positionTag, string markdownLineParagraph)
    {
        if (tagType == MarkdownTagType.Heading && validator.IsValidTag(tagType, positionTag, markdownLineParagraph))
        {
            return new MarkdownTag(tagType, positionTag, 1);
        }

        return null;
    }
    
    private (MarkdownTag openingTag, MarkdownTag closingTag)? SearchPairedTags(
        int startIndexForSearch,
        MarkdownTagType tagType,
        int positionOpenTag, 
        HashSet<int> positionsParsedTags, 
        (int position, MarkdownTagType type)[] tagsPosition,
        string markdownLineParagraph,
        List<(MarkdownTag openTag, MarkdownTag closeTag)> parsedItalicsTags)
    {
        
        var length = tagType == MarkdownTagType.Bold ? 2 : 1;
        
        for (var j = startIndexForSearch; j < tagsPosition.Length; j++)
        {
            MarkdownTagType? externalTagType = null;
                
            if (tagsPosition[j].type != tagType || positionsParsedTags.Contains(j))
                continue;
                
            var closePosition = tagsPosition[j].position;

            if (tagType == MarkdownTagType.Bold && IsHasExternalTag(positionOpenTag, closePosition, parsedItalicsTags))
                externalTagType = MarkdownTagType.Italics;
                
            if (!validator.IsValidTag(tagType, positionOpenTag, markdownLineParagraph, closePosition, externalTagType)) 
                continue;
            
            positionsParsedTags.Add(closePosition);
            var openedTag = new MarkdownTag(tagType, positionOpenTag, length);
            var closedTag = new MarkdownTag(tagType, closePosition, length, true);
            
            return (openedTag, closedTag);
        }

        return null;
    }
    
    private static IEnumerable<MarkdownTag> GetDisjointTags(
        List<(MarkdownTag openTag, MarkdownTag closeTag)> italicsTags,
        List<(MarkdownTag openTag, MarkdownTag closeTag)> boldTags)
    {
        var intersectBoldTags = new HashSet<MarkdownTag>();
        
        foreach (var (italicsOpenTag, italicsCloseTag) in italicsTags)
        {
            var isIntersect = false; 
            
            foreach (var (boldOpenTag, boldCLoseTag) in boldTags)
            {
                if (!IsOverlappingTags(italicsOpenTag, italicsCloseTag, boldOpenTag, boldCLoseTag)) 
                    continue;
                
                isIntersect = true;
                intersectBoldTags.Add(boldOpenTag);
                intersectBoldTags.Add(boldCLoseTag);
            }

            if (isIntersect) 
                continue;
            
            yield return italicsOpenTag;
            yield return italicsCloseTag;
        }
        
        foreach (var (boldOpenTag, boldCLoseTag) in boldTags)
        {
            if (intersectBoldTags.Contains(boldOpenTag) || intersectBoldTags.Contains(boldCLoseTag)) 
                continue;
            yield return boldOpenTag;
            yield return boldCLoseTag;
        }
    }

    private static bool IsOverlappingTags(
        MarkdownTag firstTagOpen, 
        MarkdownTag firstTagClose, 
        MarkdownTag secondTagOpen, 
        MarkdownTag secondTagClose)
    {
        var isFirstTagExternalToSecond = firstTagOpen.Position < secondTagOpen.Position &&
                                         secondTagClose.Position < firstTagClose.Position;
        
        var isSecondTagExternalToFirst = secondTagOpen.Position < firstTagOpen.Position &&
                                         firstTagClose.Position < secondTagClose.Position;

        if (isFirstTagExternalToSecond || isSecondTagExternalToFirst) 
            return false;


        return (firstTagOpen.Position < secondTagOpen.Position && secondTagOpen.Position < firstTagClose.Position) ||
               (secondTagOpen.Position < firstTagOpen.Position && firstTagOpen.Position < secondTagClose.Position);
    }
    
    private static bool IsHasExternalTag(int positionOpenTag, int positionCloseTag,
        List<(MarkdownTag openTag, MarkdownTag closeTag)> addedTags)
    {
        foreach (var (openTag, closeTag) in addedTags)
        {
            if (openTag.Position < positionOpenTag && positionCloseTag < closeTag.Position)
                return true;
        }

        return false;
    }
    
    private static MarkdownTagType DefineTagType(int position, string markdownLineParagraph)
    {
        return markdownLineParagraph[position] switch
        {
            '#' => MarkdownTagType.Heading,
            '_' when position < markdownLineParagraph.Length - 1 && markdownLineParagraph[position + 1] == '_' =>
                MarkdownTagType.Bold,
            _ => MarkdownTagType.Italics
        };
    }
    
    private static IEnumerable<(int Position, MarkdownTagType TagType)> SearchPositions(string markdownLineParagraph)
    {
        for (var i = 0; i < markdownLineParagraph.Length; i++)
        {
            if (markdownLineParagraph[i] != '#' && markdownLineParagraph[i] != '_')
                continue;

            var tagType = DefineTagType(i, markdownLineParagraph);
            
            yield return (i, tagType);

            if (tagType is MarkdownTagType.Bold) 
                i++;
        }
    }
}