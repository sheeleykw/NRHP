using System;

namespace NRHP_App
{
    public class ObjectBind
    {
        public String objectName;
        public bool objectState;

        public ObjectBind(String name, bool state)
        {
            objectName = name;
            objectState = state;
        }
    }
}
