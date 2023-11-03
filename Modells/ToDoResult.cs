namespace React__User_Control__API.Modells
{
    public class ToDoResult {
        public bool success;
        public string error;
        public object? result;


        public ToDoResult(bool _success, string _error, object _result) {
            success = _success;
            error = _error;
            result = _result;
        }

        public ToDoResult(bool _success, string _error) {
            success = _success;
            error = _error;
            
        }

    }
}
