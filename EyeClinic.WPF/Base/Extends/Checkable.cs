namespace EyeClinic.WPF.Base.Extends
{
    public class Checkable<T> : Bindable where T : new()
    {
        public bool IsChecked { get; set; }
        public T Item { get; set; }


        // Used only for Test Selection, 
        public bool RightEye { get; set; } = true;
        public bool LeftEye { get; set; } = true;
    }
}
