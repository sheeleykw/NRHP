using System;

namespace NRHP_App
{
    public class StateBind
    {
        public String StateName;
        public bool StateBinding;

        public StateBind(String stateName, bool stateBinding)
        {
            StateName = stateName;
            StateBinding = stateBinding;
        }
    }
}
