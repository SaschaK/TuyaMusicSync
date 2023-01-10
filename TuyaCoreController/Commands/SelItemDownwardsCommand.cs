using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TuyaCoreController.ViewModel;

namespace TuyaCoreController.Commands
{
    public class SelItemDownwardsCommand : ICommand
    {
        public event EventHandler CanExecuteChanged
        {
            add { }
            remove { }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (OwnDataContext.Instance.SelectedIdxSelLights > -1 && OwnDataContext.Instance.SelectedIdxSelLights < OwnDataContext.Instance.SelectedLights.Count - 1)
                OwnDataContext.Instance.SelectedLights.Move(OwnDataContext.Instance.SelectedIdxSelLights, OwnDataContext.Instance.SelectedIdxSelLights + 1);
        }
    }
}
