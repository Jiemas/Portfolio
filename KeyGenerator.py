import requests
import json
from datetime import datetime

FIREBASE_URL = "https://portfolio-a3134-default-rtdb.firebaseio.com"

def sort_by_time(data):
    # Helper: convert "June 2024" → datetime object
    def parse_date(date_str):
        return datetime.strptime(date_str, "%B %Y")

    # Convert dict → list of (key, obj) pairs
    items = list(data.items())

    # Sort by end date (newest first), then start date (newest first)
    items.sort(
        key=lambda item: (
            parse_date(item[1]["TimeWorked"][1]),  # end date
            parse_date(item[1]["TimeWorked"][0])   # start date
        ),
        reverse=True
    )

    # Build result list without TimeWorked
    result = []
    for key, obj in items:
        filtered = {k: v for k, v in obj.items() if k != "TimeWorked"}
        result.append(filtered)

    return result

find_index = lambda l, target: next((i for i, d in enumerate(l) if d.get("Link") == target), -1)

def build_keys_from_skills(skill_key_list):
    resp = requests.get(f"{FIREBASE_URL}/.json")
    resp.raise_for_status()
    data = resp.json()

    skills = data.get("Skills", {})
    experiences = data.get("Experiences", {})
    projects = data.get("Projects", {})

    keys_obj = {}

    keys_obj["AboutMeKey"] = "simple"
    keys_obj["SkillKeys"] = skill_key_list

    experiences_dict = dict()
    projects_dict = dict()

    for skill_key in skill_key_list:
        skill = skills.get(skill_key)
        if not skill:
            # skill not found — skip or warn
            print(f"{skill_key}: Warning: skill key '{skill_key}' not found in Skills.")
            continue

        # process Experience links
        for se in skill.get("SkillExperiences", []):
            link = se.get("Link")
            if not link or link == "na":
                continue
            if link not in experiences:
                print(f"{skill_key}: Warning: experience link '{link}' not found under Experiences.")
                continue
            
            experience = experiences.get(link)
            experience_skills_used = experience.get("SkillsUsed", [])
            experience_skill_index = find_index(experience_skills_used, skill_key)
            if (experience_skill_index < 0):
                print(f"{skill_key}: Warning: skill not found in {link}'s SkillsUsed list. Adding to list")
                data = {len(experience_skills_used): {"Link": skill_key, "Text": skill["SkillName"]}}
                requests.patch(f"{FIREBASE_URL}/Experiences/{link}/SkillsUsed.json", json=data)
                experience_skill_index = len(experience_skills_used)
                experience_skills_used.append({"Link": skill_key, "Text": skill["SkillName"]})
            
            if link not in experiences_dict:
                experiences_dict[link] = {
                  "key": link, 
                  "RelatedWorkKeys": list(range(len(experience["RelatedWork"]))),
                  "SkillsUsedKeys": [],
                  "TimeWorked": experience["BasicInformation"]["TimeWorked"]
                }
            experiences_dict[link]["SkillsUsedKeys"].append(experience_skill_index)

        # process Project links
        for sp in skill.get("SkillProjects", []):
            link = sp.get("Link")
            if not link or link == "na":
                continue
            if link not in projects:
                print(f"{skill_key}: Warning: project link '{link}' not found under Projects.")
                continue
            
            project = projects.get(link)
            project_skills_used = project.get("SkillsUsed", [])
            project_skill_index = find_index(project_skills_used, skill_key)
            
            if (project_skill_index < 0):
                print(f"{skill_key}: Warning: skill not found in {link}'s SkillsUsed list. Adding to list")
                data = {len(project_skills_used): {"Link": skill_key, "Text": skill["SkillName"]}}
                requests.patch(f"{FIREBASE_URL}/Projects/{link}/SkillsUsed.json", json=data)
                project_skill_index = len(project_skills_used)
                project_skills_used.append({"Link": skill_key, "Text": skill["SkillName"]})

            if link not in projects_dict:
                projects_dict[link] = {
                  "key": link, 
                  "BulletPointKeys": [0],
                  "SkillsUsedKeys": [],
                  "TimeWorked": project["Overview"]["TimeWorked"]
                }
            projects_dict[link]["SkillsUsedKeys"].append(project_skill_index)
    keys_obj["ExperienceKeys"] = sort_by_time(experiences_dict)
    keys_obj["ProjectKeys"] = sort_by_time(projects_dict)
    # print(sort_by_time(projects_dict))
    return keys_obj

def save_json_to_file(data, filename="generated_keys.json"):
    """Save JSON data to a file with pretty formatting."""
    with open(filename, "w", encoding="utf-8") as f:
        json.dump(data, f, indent=2, ensure_ascii=False)
    print(f"Saved output to {filename}")

if __name__ == "__main__":
    # example usage
    # Node.js, JavaScript, Python, PostgreSQL, Linode, Docker, RESTFul APIs, Redis, Git, 

    my_skills = ["swebapp", "scommunication", "sj", "sr", "shtml", "scss", "sjson", "srest", "sgit", "steamwork", "scsharp", "sp", "sllm", "sarcgis", "swcag", "ss"]  # these should match your Skills keys (not skill-names)
    keys = build_keys_from_skills(my_skills)
    save_json_to_file(keys)
    # print(json.dumps(keys, indent=2))
