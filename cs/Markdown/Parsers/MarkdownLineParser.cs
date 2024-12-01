using System.Text;
using Markdown.MarkdownTags;
using Markdown.MarkdownTagValidators;

namespace Markdown.Parsers;

public class MarkdownLineParser : IMarkdownLineParser
{
    private readonly IMarkdownTagValidator markdownTagValidator;

    public MarkdownLineParser(IMarkdownTagValidator markdownTagValidator)
    {
        this.markdownTagValidator = markdownTagValidator;
    }

    public IEnumerable<MarkdownTag> ParseMarkdownTags(string markdownLineParagraph)
    {
        var parsedTags = new List<MarkdownTag>();
        var tagsPosition = SearchTagPositionsAndTypes(markdownLineParagraph).ToArray();
        var positionsParsedTags = new HashSet<int>();
        var parsedBoldTags = new List<(MarkdownTag openTag, MarkdownTag closeTag)>();
        var parsedItalicsTags = new List<(MarkdownTag openTag, MarkdownTag closeTag)>();

        for (var i = 0; i < tagsPosition.Length; i++)
        {
            var tagType = tagsPosition[i].TagType;
            var positionTag = tagsPosition[i].Position;
            
            if (tagType == MarkdownTagType.Heading && markdownTagValidator.IsValidTag(tagType, positionTag, markdownLineParagraph))
            {
                parsedTags.Add(new MarkdownTag(tagType, positionTag, 1));
                continue;
            }

            var pairedTags = SearchPairedTags(
                i + 1,
                tagType,
                positionTag,
                positionsParsedTags,
                tagsPosition,
                markdownLineParagraph,
                parsedItalicsTags);
            
            if (pairedTags == null) 
                continue;
                
            if (tagType == MarkdownTagType.Bold)
                parsedBoldTags.Add(pairedTags.Value);
            else
                parsedItalicsTags.Add(pairedTags.Value);
        }

        var disjointTags = GetDisjointTags(parsedItalicsTags, parsedBoldTags);
        parsedTags.AddRange(disjointTags);
        
        return parsedTags.OrderBy(tag => tag.Position);
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
               markdownTagValidator.IsValidTag(MarkdownTagType.Heading, line.Length - trimStartLine.Length, line);
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
                
            if (!markdownTagValidator.IsValidTag(tagType, positionOpenTag, markdownLineParagraph, closePosition, externalTagType)) 
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
    
    private static IEnumerable<(int Position, MarkdownTagType TagType)> SearchTagPositionsAndTypes(
        string markdownLineParagraph)
    {
        var tagPositionsAndTypes = new List<(int Position, MarkdownTagType TagType)>();
        var isItalicsTagFollowBoldTag = false;
        
        for (var i = 0; i < markdownLineParagraph.Length; i++)
        {
            if (markdownLineParagraph[i] != '#' && markdownLineParagraph[i] != '_')
                continue;

            var tagType = DefineTagType(i, markdownLineParagraph);
            tagPositionsAndTypes.Add((i, tagType));
            
            switch (tagType)
            {
                case MarkdownTagType.Italics when IsItalicsTagFollowBoldTag(tagPositionsAndTypes, i):
                {
                    if (isItalicsTagFollowBoldTag)
                    {
                        ChangeItalicsAndBoldTagsPositions(tagPositionsAndTypes);
                        isItalicsTagFollowBoldTag = false;
                    }
                    else
                        isItalicsTagFollowBoldTag = true;

                    break;
                }
                case MarkdownTagType.Bold:
                    i++;
                    break;
            }
        }

        return tagPositionsAndTypes;
    }

    private static bool IsItalicsTagFollowBoldTag(
        List<(int Position, MarkdownTagType TagType)> tagPositionsAndTypes,
        int italicsTagPosition)
    {
        if (tagPositionsAndTypes.Count < 2)
            return false;

        return tagPositionsAndTypes[^2].TagType == MarkdownTagType.Bold &&
               tagPositionsAndTypes[^2].Position == italicsTagPosition - 2;
    }

    private static void ChangeItalicsAndBoldTagsPositions(
        List<(int Position, MarkdownTagType TagType)> tagPositionsAndTypes)
    {
        var italicsTagPosition = tagPositionsAndTypes[^1].Position;

        for (var i = tagPositionsAndTypes.Count - 2; i >= 0; i++)
        {
            var currentTagPosition = tagPositionsAndTypes[i].Position;
            
            if (currentTagPosition != italicsTagPosition - 2 || tagPositionsAndTypes[i].TagType != MarkdownTagType.Bold)
                break;

            tagPositionsAndTypes[i] = (currentTagPosition, MarkdownTagType.Italics);
            tagPositionsAndTypes[i + 1] = (currentTagPosition + 1, MarkdownTagType.Bold);
        }
    }
}