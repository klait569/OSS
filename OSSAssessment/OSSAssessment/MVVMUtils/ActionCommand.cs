﻿using System;
using System.Windows.Input;

namespace OSSAssessment
{
    /// <summary>
    /// Defines an action command.
    /// </summary>
    public class ActionCommand : ICommand
    {
        public delegate bool CanExecuteDelegate(object parameter);

        private readonly Action<object> _actionExecute;
        private readonly CanExecuteDelegate _actionCanExecute;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="actionExecute">The action to execute.</param>
        /// <param name="actionCanExecute">Parameter indicating if the action can be executed.</param>
        public ActionCommand(Action<object> actionExecute, CanExecuteDelegate actionCanExecute = null)
        {
            _actionExecute = actionExecute;
            _actionCanExecute = actionCanExecute;
        }

        /// <summary>
        /// Defines the method that determines whether the command can be executed
        //  in the given context.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        /// <returns>True if the action can be executed in the given context.</returns>
        public bool CanExecute(object parameter)
        {
            return _actionCanExecute == null || _actionCanExecute(parameter);
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.</param>
        public void Execute(object parameter)
        {
            _actionExecute(parameter);
        }

        /// <summary>
        /// Called when the current state of the application is changed
        /// and the condition of executing a command could be changed.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, null);
        }

        public event EventHandler CanExecuteChanged;
    }
}