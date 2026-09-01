using System.Diagnostics.CodeAnalysis;
using axis_mobile_app.Models;

namespace axis_mobile_app.Utilities;

/// <summary>
/// Project Model Extensions
/// </summary>
public static class ProjectExtensions
{
    /// <summary>
    /// Check if the project is null or new.
    /// </summary>
    /// <param name="project"></param>
    /// <returns></returns>
    public static bool IsNullOrNew([NotNullWhen(false)] this Project? project)
    {
        return project is null || project.ID == 0;
    }
}