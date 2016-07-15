package md5eef3c13c0fec5892e1dbe30ce70f8ab4;


public class activity_splash
	extends android.support.v7.app.AppCompatActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Morris.activity_splash, Morris, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", activity_splash.class, __md_methods);
	}


	public activity_splash () throws java.lang.Throwable
	{
		super ();
		if (getClass () == activity_splash.class)
			mono.android.TypeManager.Activate ("Morris.activity_splash, Morris, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
