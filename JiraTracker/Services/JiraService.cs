using Atlassian.Jira;
using JiraTracker.Interaces;
using System.Globalization;
using System.Text;

namespace JiraTracker.Services
{
    public class JiraService : IJiraService
    {
        private Jira jira;
        private Project? project;

        public JiraService()
        {
            Console.WriteLine("JiraService created");
        }

        public void Initialize(string url, string login, string token, string projectKey)
        {
            this.jira = Jira.CreateRestClient(url, login, token);

            var projects = this.jira.Projects.GetProjectsAsync().Result;

            if (projects.Count() == 0)
            {
                throw new Exception("No project found");
            }

            this.project = projects.Where(p => projectKey == p.Key).FirstOrDefault();

            if (this.project == null)
            {
                throw new Exception("Project not found");
            }

            Console.WriteLine("Project found: " + this.project.Name);
        }
        public async Task<bool> AddWorklog(string name, DateTime start, DateTime end)
        {
            Console.WriteLine($"AddWorklog: {name} {start} {end}");
            string cleanName = RemoveDiacritics(name.Trim().ToLower());
            Issue? issue;
            // find the issue by name
            switch (cleanName)
            {
                case "reunion":
                    issue = await FindRitualWithName("Réunion");
                    break;
                case "ds":
                case "daily":
                case "daily scrum":
                    issue = await FindRitualWithName("Daily Scrum");
                    break;
                case "gestion de projet":
                    issue = await FindRitualWithName("Gestion de projet");
                    break;
                case "autre":
                    issue = await FindRitualWithName("Autre");
                    break;
                case "livraison":
                    issue = await FindRitualWithName("Livraison");
                    break;
                case "pause":
                    issue = null;
                    break;
                default:
                    issue = await FindIssueByKey(name);
                    break;
            }

            Console.WriteLine($"Issue found: {issue?.Key}");

            if (issue != null)
            {
                await AddWorklogToIssue(issue, start, end);
                return true;
            }
            return false;
        }

        private async Task AddWorklogToIssue(Issue issue, DateTime start, DateTime end)
        {
            var timespan = end - start;
            var worklog = new Worklog(TimespanToJiraTimeFormat(timespan), start);

            await issue.AddWorklogAsync(worklog);
        }

        private async Task<Issue?> FindRitualWithName(string name)
        {
            if (this.project == null)
            {
                throw new Exception("Project not initialized");
            }
            string jql = $"project = \"{this.project.Key}\" AND status = \"In Progress\" AND issuetype = Rituels AND summary ~ \"{name}\"";
            Console.WriteLine($"FindRitualWithName: {jql}");
            var issues = await this.jira.Issues.GetIssuesFromJqlAsync(jql);
            Console.WriteLine($"Issues found: {issues.Count()}");
            foreach (var issue in issues)
            {
                if (issue.Summary.ToLower().Contains(name.ToLower()))
                {
                    return issue;
                }
            }
            return null;
        }

        private async Task<Issue?> FindIssueByKey(string key)
        {
            return await this.jira.Issues.GetIssueAsync(key);
        }

        private static string TimespanToJiraTimeFormat(TimeSpan timeSpan)
        {
            return $"{timeSpan.Hours}h {timeSpan.Minutes}m";
        }

        private static string RemoveDiacritics(string text)
        {
            string normalizedString = text.Normalize(NormalizationForm.FormD);

            var stringBuilder = new StringBuilder(normalizedString.Length);

            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString();
        }
    }
}