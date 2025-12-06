public class ExpandedStates
{
  public bool SkillExpanded { get; set; }
  public bool ExperienceExpanded { get; set; }
  public bool ProjectExpanded { get; set; }
  public event Action SkillChange = null!;
  public event Action ExperienceChange = null!;
  public event Action ProjectChange = null!;
  public List<string>? skillKeys {get; set; }
  public List<ExperienceKey>? experienceKeys {get; set; }
  public List<ProjectKey>? projectKeys {get; set; }

  public void HandleAnchorClick(string data)
  {
    if (data[0] == 's' && skillKeys is not null && skillKeys.Count > 3 && !skillKeys.GetRange(0, 3).Contains(data))
    {
      Console.WriteLine($"{data} not found within first three skills");
      SkillExpanded = true;
      SkillChange?.Invoke();
    } else if (data[0] == 'e' && experienceKeys is not null && experienceKeys.Count > 2 && experienceKeys[0].Key != data && experienceKeys[1].Key != data)
    {
      ExperienceExpanded = true;
      ExperienceChange?.Invoke();
    } else if (data[0] == 'p' && projectKeys is not null && projectKeys[0].Key != data)
    {
      ProjectExpanded = true;
      ProjectChange?.Invoke();
    }
  }
}