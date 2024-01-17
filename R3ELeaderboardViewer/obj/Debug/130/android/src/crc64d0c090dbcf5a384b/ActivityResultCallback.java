package crc64d0c090dbcf5a384b;


public class ActivityResultCallback
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		androidx.activity.result.ActivityResultCallback
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onActivityResult:(Ljava/lang/Object;)V:GetOnActivityResult_Ljava_lang_Object_Handler:AndroidX.Activity.Result.IActivityResultCallbackInvoker, Xamarin.AndroidX.Activity\n" +
			"";
		mono.android.Runtime.register ("R3ELeaderboardViewer.Fragments.ActivityResultCallback, R3ELeaderboardViewer", ActivityResultCallback.class, __md_methods);
	}


	public ActivityResultCallback ()
	{
		super ();
		if (getClass () == ActivityResultCallback.class) {
			mono.android.TypeManager.Activate ("R3ELeaderboardViewer.Fragments.ActivityResultCallback, R3ELeaderboardViewer", "", this, new java.lang.Object[] {  });
		}
	}


	public void onActivityResult (java.lang.Object p0)
	{
		n_onActivityResult (p0);
	}

	private native void n_onActivityResult (java.lang.Object p0);

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
