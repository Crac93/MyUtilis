﻿using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtilis.Windows
{
    /// <summary>
    /// Crear tareas uatomaticas en Windows
    /// </summary>
    public class TaskScheduler
    {
        /// <summary>
        /// Crea una tarea automatica por dia...
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="pathExe"></param>
        /// <param name="startHour"></param>
        /// <param name="daysInterval"></param>
        public static void CreateTaskDaily(string name, string description, string pathExe, double startHour, short daysInterval)
        {
            // Get the service on the local machine
            using (TaskService ts = new TaskService())
            {
                string taskName = name;

                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = description;

                DailyTrigger dTrigger = td.Triggers.Add(new DailyTrigger());
                dTrigger.StartBoundary = DateTime.Today + TimeSpan.FromHours(startHour);
                dTrigger.DaysInterval = daysInterval;

                td.Actions.Add(new ExecAction(pathExe));
                td.Settings.MultipleInstances = TaskInstancesPolicy.StopExisting;
                ts.RootFolder.RegisterTaskDefinition(taskName, td, TaskCreation.CreateOrUpdate, null, null, TaskLogonType.InteractiveToken, null);

                //SHOW ALL INFORAMTION OF NEW TASKs
                TaskFolder tf = ts.RootFolder;
                Microsoft.Win32.TaskScheduler.Task runningTask = tf.Tasks[taskName];
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("New task will run at:" + runningTask.NextRunTime);
                Console.WriteLine("New task triggers:");
                for (int i = 0; i < runningTask.Definition.Triggers.Count; i++)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("{0}", runningTask.Definition.Triggers[i]);
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("New task actions:");
                for (int i = 0; i < runningTask.Definition.Actions.Count; i++)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("{0}", runningTask.Definition.Actions[i]);
                }
            }
        }
    
        /// <summary>
        /// Crea una tarea mas especifica
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="pathExe"></param>
        /// <param name="startHour"></param>
        /// <param name="daysInterval"></param>
        /// <param name="allowRun"></param>
        /// <param name="argum"></param>
        /// <returns></returns>
        public static string CreateTask(string name, string description, string pathExe, double startHour, short daysInterval, bool allowRun = false, string argum = "")
        {
            string response = "";
            // Get the service on the local machine
            using (TaskService ts = new TaskService())
            {
                string taskName = name;

                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = description;

                DailyTrigger dTrigger = td.Triggers.Add(new DailyTrigger());

                if (startHour != 0)
                    dTrigger.StartBoundary = DateTime.Today + TimeSpan.FromHours(startHour);

                if (daysInterval != 0)
                    dTrigger.DaysInterval = daysInterval;

                if (allowRun)
                    dTrigger.Repetition.Interval = TimeSpan.FromSeconds(60);

                td.Settings.MultipleInstances = TaskInstancesPolicy.StopExisting;

                td.Actions.Add(new ExecAction(pathExe, argum));

                ts.RootFolder.RegisterTaskDefinition(taskName, td, TaskCreation.CreateOrUpdate, null, null, TaskLogonType.InteractiveToken, null);

                //SHOW ALL INFORAMTION OF NEW TASKs
                TaskFolder tf = ts.RootFolder;
                Microsoft.Win32.TaskScheduler.Task runningTask = tf.Tasks[taskName];

                response = "==============================" + Environment.NewLine;
                response += "New task:" + taskName + Environment.NewLine;
                response += "Triggers:";

                for (int i = 0; i < runningTask.Definition.Triggers.Count; i++)
                {
                    response += string.Format("{0}", runningTask.Definition.Triggers[i]) + Environment.NewLine;
                }

                response += "Actions:" + Environment.NewLine;

                for (int i = 0; i < runningTask.Definition.Actions.Count; i++)
                {
                    response += string.Format("{0}", runningTask.Definition.Actions[i]) + Environment.NewLine;
                    response += Environment.NewLine;
                }

                return response;
            }
        }


    }//CLASS
}//NAMESPACE
