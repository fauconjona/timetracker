using Atlassian.Jira;
using JiraTracker.Interaces;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace JiraTracker.Services
{
    public class JiraService : IJiraService
    {
        private Jira jira;
        private Project? project;
        private Dictionary<string, string> Aliases = new Dictionary<string, string>();

        public JiraService()
        {
            Console.WriteLine("JiraService created");
        }

        public void Initialize(string url, string login, string token, string projectKey, Dictionary<string, string>? aliases = null)
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

            if (aliases != null)
            {
                this.Aliases = aliases;
            }

            Console.WriteLine("Project found: " + this.project.Name);
        }
        public async Task<bool> AddWorklog(string key, DateTime start, DateTime end)
        {
            Console.WriteLine($"AddWorklog: {key} {start} {end}");
            string ticket = key;

            if (Aliases.ContainsKey(key))
            {
                ticket = Aliases[key];
            }

            Issue? issue = await FindIssueByKey(ticket);

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