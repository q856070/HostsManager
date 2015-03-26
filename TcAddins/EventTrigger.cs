using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE80;
using EnvDTE;

namespace TcAddins {
    public class EventTrigger : IDisposable {

        public delegate void SolutionEvents_OpenedEventHandler();


        public event SolutionEvents_OpenedEventHandler Opened;
        public void Start() {
            if (_checkThd == null) {
                _checkThd = new System.Threading.Thread(new System.Threading.ThreadStart(EventCheck));
            }
            if (_checkThd != null && _checkThd.ThreadState == System.Threading.ThreadState.Unstarted) {
                _checkThd.Start();
                _solutionOpenedCount = _applicationObject.Solution.Count;
            }
        }
        public void End() {
            if (_checkThd != null) {
                try {
                    _checkThd.Abort();
                } catch { }
                _checkThd = null;
               
            }
        }
        #region IDisposable 成员

        public void Dispose() {
            End();
        }

        #endregion

        private DTE2 _applicationObject;
        private AddIn _addInInstance;

        private System.Threading.Thread _checkThd = null;
        private System.ComponentModel.AsyncOperation _ayOp = System.ComponentModel.AsyncOperationManager.CreateOperation(null);

        public EventTrigger(DTE2 applicationObject, AddIn addInInstance) {
            _applicationObject = applicationObject;
            _addInInstance = addInInstance;
        }


        private int _solutionOpenedCount = 0;
        private void EventCheck() {
            while (true) {
                try {
                    if (_solutionOpenedCount != _applicationObject.Solution.Count) {
                        //解决方案打开或关闭了
                        if (_applicationObject.Solution.Count > _solutionOpenedCount) {
                            //解决方案打开
                            _ayOp.Post(delegate {
                                SolutionEvents_OpenedEventHandler opened = Opened;
                                if (opened != null) {
                                    opened();
                                }
                            }, null);
                        }
                        _solutionOpenedCount = _applicationObject.Solution.Count;
                    }
                } catch { }

                System.Threading.Thread.Sleep(100);
            }

        }



    }
}
