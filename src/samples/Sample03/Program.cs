﻿using System;
using System.Threading.Tasks;
using Elsa;
using Elsa.Activities.Console.Activities;
using Elsa.Activities.Console.Extensions;
using Elsa.Expressions;
using Elsa.Models;
using Elsa.Services;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace Sample03
{
    /// <summary>
    /// A minimal workflows program defined as data (workflow definition) and Console activities.
    /// </summary>
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            // Setup a service collection.
            var services = new ServiceCollection()
                .AddElsa()
                .AddConsoleActivities()
                .BuildServiceProvider();

            // Define a workflow as data so we can store it somewhere (file, database, etc.).
            var workflowDefinition = new WorkflowDefinitionVersion
            {
                Activities = new[]
                {
                    new ActivityDefinition<WriteLine> { Id = "activity-1", State = JObject.FromObject(new { TextExpression = new LiteralExpression("Hello world!")})},
                    new ActivityDefinition<WriteLine> { Id = "activity-2", State = JObject.FromObject(new { TextExpression = new LiteralExpression("Goodbye cruel world...")})}
                },
                Connections = new []
                {
                    new ConnectionDefinition("activity-1", "activity-2", OutcomeNames.Done), 
                }
            };
            
            // Run the workflow.
            var invoker = services.GetService<IWorkflowRunner>();
            await invoker.RunAsync(workflowDefinition);

            Console.ReadLine();
        }
    }
}