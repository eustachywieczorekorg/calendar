using Android.App;
using Android.OS;
using Android.Views;

namespace Morris
{
    public class SupportFragment : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            var view = inflater.Inflate(Resource.Layout.dialog_addevent, container, false);

            return view;
            
        }
    }
}