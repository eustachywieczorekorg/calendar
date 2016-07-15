package md5eef3c13c0fec5892e1dbe30ce70f8ab4;


public class activity_calendar
	extends android.support.v4.app.Fragment
	implements
		mono.android.IGCUserPeer,
		android.widget.CalendarView.OnDateChangeListener,
		android.widget.DatePicker.OnDateChangedListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreateView:(Landroid/view/LayoutInflater;Landroid/view/ViewGroup;Landroid/os/Bundle;)Landroid/view/View;:GetOnCreateView_Landroid_view_LayoutInflater_Landroid_view_ViewGroup_Landroid_os_Bundle_Handler\n" +
			"n_onCreateOptionsMenu:(Landroid/view/Menu;Landroid/view/MenuInflater;)V:GetOnCreateOptionsMenu_Landroid_view_Menu_Landroid_view_MenuInflater_Handler\n" +
			"n_onOptionsItemSelected:(Landroid/view/MenuItem;)Z:GetOnOptionsItemSelected_Landroid_view_MenuItem_Handler\n" +
			"n_onSelectedDayChange:(Landroid/widget/CalendarView;III)V:GetOnSelectedDayChange_Landroid_widget_CalendarView_IIIHandler:Android.Widget.CalendarView/IOnDateChangeListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"n_onDateChanged:(Landroid/widget/DatePicker;III)V:GetOnDateChanged_Landroid_widget_DatePicker_IIIHandler:Android.Widget.DatePicker/IOnDateChangedListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("Morris.activity_calendar, Morris, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", activity_calendar.class, __md_methods);
	}


	public activity_calendar () throws java.lang.Throwable
	{
		super ();
		if (getClass () == activity_calendar.class)
			mono.android.TypeManager.Activate ("Morris.activity_calendar, Morris, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public android.view.View onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2)
	{
		return n_onCreateView (p0, p1, p2);
	}

	private native android.view.View n_onCreateView (android.view.LayoutInflater p0, android.view.ViewGroup p1, android.os.Bundle p2);


	public void onCreateOptionsMenu (android.view.Menu p0, android.view.MenuInflater p1)
	{
		n_onCreateOptionsMenu (p0, p1);
	}

	private native void n_onCreateOptionsMenu (android.view.Menu p0, android.view.MenuInflater p1);


	public boolean onOptionsItemSelected (android.view.MenuItem p0)
	{
		return n_onOptionsItemSelected (p0);
	}

	private native boolean n_onOptionsItemSelected (android.view.MenuItem p0);


	public void onSelectedDayChange (android.widget.CalendarView p0, int p1, int p2, int p3)
	{
		n_onSelectedDayChange (p0, p1, p2, p3);
	}

	private native void n_onSelectedDayChange (android.widget.CalendarView p0, int p1, int p2, int p3);


	public void onDateChanged (android.widget.DatePicker p0, int p1, int p2, int p3)
	{
		n_onDateChanged (p0, p1, p2, p3);
	}

	private native void n_onDateChanged (android.widget.DatePicker p0, int p1, int p2, int p3);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
