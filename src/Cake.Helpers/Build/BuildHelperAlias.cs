﻿using Cake.Core.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Helpers.Clean;
using Cake.Helpers.Tasks;

namespace Cake.Helpers.Build
{
  /// <summary>
  /// Cake Generic Build Task Aliases
  /// </summary>
  [CakeAliasCategory("Helper")]
  [CakeAliasCategory("Build")]
  public static class BuildHelperAlias
  {
    /// <summary>
    /// Gets Build Clean Task for defined parameters
    /// </summary>
    /// <param name="context">Cake Context</param>
    /// <param name="taskName">Target Name</param>
    /// <param name="isTarget">True=Public Target(Listed),False=Private Target(Not Listed)</param>
    /// <param name="parentTaskName">Parent Target name. Required if isTarget is False</param>
    /// <returns>Task</returns>
    /// <example>
    /// <code>
    /// // Creates task "Clean-Build-Sln"
    /// BuildCleanTask("Sln", true)
    ///   .Does(() => { "Clean Solution" });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    public static CakeTaskBuilder<ActionTask> BuildCleanTask(
      this ICakeContext context,
      string taskName,
      bool isTarget = true,
      string parentTaskName = "")
    {
      if (context == null)
        throw new ArgumentNullException(nameof(context));

      return context.TaskHelper()
        .AddToBuildCleanTask(taskName, isTarget, parentTaskName)
        .GetTaskBuilder();
    }

    /// <summary>
    /// Gets PreBuild task for defined parameters. Runs associated BuildClean task as dependency always
    /// </summary>
    /// <param name="context">Cake Context</param>
    /// <param name="taskName">Target Name</param>
    /// <param name="isTarget">True=Public Target(Listed),False=Private Target(Not Listed)</param>
    /// <param name="parentTaskName">Parent Target name. Required if isTarget is False</param>
    /// <returns>Task</returns>
    /// <example>
    /// <code>
    /// // Creates task "PreBuild-Sln"
    /// PreBuildTask("Sln", true)
    ///   .Does(() => { "Restore Solution" });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    public static CakeTaskBuilder<ActionTask> PreBuildTask(
      this ICakeContext context,
      string taskName,
      bool isTarget = true,
      string parentTaskName = "")
    {
      if (context == null)
        throw new ArgumentNullException(nameof(context));

      return context.TaskHelper()
        .AddToPreBuildTask(taskName, isTarget, parentTaskName)
        .GetTaskBuilder();
    }

    /// <summary>
    /// Gets Build task for defined parameters. Runs associated PreBuild task as dependency always
    /// </summary>
    /// <param name="context">Cake Context</param>
    /// <param name="taskName">Target Name</param>
    /// <param name="isTarget">True=Public Target(Listed),False=Private Target(Not Listed)</param>
    /// <param name="parentTaskName">Parent Target name. Required if isTarget is False</param>
    /// <returns>Task</returns>
    /// <example>
    /// <code>
    /// // Creates task "Build-Sln"
    /// BuildTask("Sln", true)
    ///   .Does(() => { "Build Solution" });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    public static CakeTaskBuilder<ActionTask> BuildTask(
      this ICakeContext context,
      string taskName,
      bool isTarget = true,
      string parentTaskName = "")
    {
      if (context == null)
        throw new ArgumentNullException(nameof(context));

      return context.TaskHelper()
        .AddToBuildTask(taskName, isTarget, parentTaskName)
        .GetTaskBuilder();
    }

    /// <summary>
    /// Gets PostBuild task for defined parameters. Runs associated Build task as dependency only if HelperSettings.RunAllDependencies = True
    /// </summary>
    /// <param name="context">Cake Context</param>
    /// <param name="taskName">Target Name</param>
    /// <param name="isTarget">True=Public Target(Listed),False=Private Target(Not Listed)</param>
    /// <param name="parentTaskName">Parent Target name. Required if isTarget is False</param>
    /// <returns>Task</returns>
    /// <example>
    /// <code>
    /// // Creates task "PostBuild-Sln"
    /// PostBuildTask("Sln", true)
    ///   .Does(() => { "Copy Artifacts somewhere" });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    public static CakeTaskBuilder<ActionTask> PostBuildTask(
      this ICakeContext context,
      string taskName,
      bool isTarget = true,
      string parentTaskName = "")
    {
      if (context == null)
        throw new ArgumentNullException(nameof(context));

      return context.TaskHelper()
        .AddToPostBuildTask(taskName, isTarget, parentTaskName)
        .GetTaskBuilder();
    }
  }
}
