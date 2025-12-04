public class FirebaseData
{
  public required Dictionary<string, Skill> Skills { get; set; }
  public required Dictionary<string, Experience> Experiences { get; set; }
  public required Dictionary<string, Key> Keys { get; set; }

  public required Dictionary<string, Project> Projects { get; set; }
}

public class Skill 
{
  public required string ImageUrl { get; set; }
  public required string ImageAlt { get; set; }
  public required string Key { get; set; }
  public required string SkillName { get; set; }

  public required string Subtitle { get; set; }

  public required List<HyperLink> SkillExperiences { get; set; }

  public required List<HyperLink> SkillProjects { get; set; }
}

public class HyperLink
{
  public required string Text { get; set; }
  public string? Link { get; set; }
}

public class Experience
{
  public required string Key { get; set; }
  public required string ExperienceName { get; set; }
  public required string ImageUrl { get; set; }
  public required string ImageAlt { get; set; }

  public required List<HyperLink> Description { get; set; }

  public required List<HyperLink> RelatedWork { get; set; }

  public required Information BasicInformation { get; set; }

  public required List<HyperLink> SkillsUsed { get; set; }
}

public class Key
{
  public required List<string> SkillKeys { get; set; }
  public required List<ExperienceKey> ExperienceKeys { get; set; }
  public required List<ProjectKey> ProjectKeys { get; set; }
}

public class Information
{
  public required string Location { get; set; }
  public required string WorkType { get; set; }
  public required string[] TimeWorked { get; set; }
  public required string RelatedOrganization { get; set; }
}

public class ExperienceKey
{
  public required string Key { get; set; }
  public required List<int> RelatedWorkKeys { get; set; }
  public required List<int> SkillsUsedKeys { get; set; }

  public required int ImageSize { get; set; }
  
  public required bool LargeContent { get; set; }
}


public class ProjectKey
{
  public required string Key { get; set; }
  public required List<int> BulletPointKeys { get; set; }
  public required List<int> SkillsUsedKeys { get; set; }
}

public class Project
{
  public required string Name { get; set; }
  public required OverviewInfo Overview { get; set; }
  public required List<HyperLink> SkillsUsed { get; set; }
  public required string Description { get; set; }
  public required string EmbedElement { get; set; }
  public required List<string> BulletPoints { get; set; }
  public required string ImageUrl { get; set; }
  public required string Key { get; set; }
  public required string ImageAlt { get; set; }
  public required string EmbedElementAlt { get; set; }
}

public class OverviewInfo
{
  public required string Role { get; set; }

  public required string RelatedOrganization { get; set; }
  public required string[] TimeWorked { get; set; }
  public required HyperLink RelatedWork { get; set; }
  public required HyperLink SourceCode { get; set; }
  public required HyperLink DeployLink { get; set; }
}