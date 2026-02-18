namespace Common.Models
{
    public class AvgData : ObservableObject
    {
         private string _id = string.Empty;
        public string ID
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private int _h;
        public int H
        {
            get => _h;
            set => SetProperty(ref _h, value);
        }

        private int _l;
        public int L
        {
            get => _l;
            set => SetProperty(ref _l, value);
        }

        private int _s;
        public int S
        {
            get => _s;
            set => SetProperty(ref _s, value);
        }

        private float _a;
        public float A
        {
            get => _a;
            set => SetProperty(ref _a, value);
        }
    }
}
