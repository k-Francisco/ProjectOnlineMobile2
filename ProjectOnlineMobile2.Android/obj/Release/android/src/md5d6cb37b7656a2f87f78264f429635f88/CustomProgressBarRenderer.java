package md5d6cb37b7656a2f87f78264f429635f88;


public class CustomProgressBarRenderer
	extends md5b60ffeb829f638581ab2bb9b1a7f4f3f.ProgressBarRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("ProjectOnlineMobile2.Android.Renderers.CustomProgressBarRenderer, ProjectOnlineMobile2.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", CustomProgressBarRenderer.class, __md_methods);
	}


	public CustomProgressBarRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == CustomProgressBarRenderer.class)
			mono.android.TypeManager.Activate ("ProjectOnlineMobile2.Android.Renderers.CustomProgressBarRenderer, ProjectOnlineMobile2.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:System.Int32, mscorlib, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public CustomProgressBarRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == CustomProgressBarRenderer.class)
			mono.android.TypeManager.Activate ("ProjectOnlineMobile2.Android.Renderers.CustomProgressBarRenderer, ProjectOnlineMobile2.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065:Android.Util.IAttributeSet, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0, p1 });
	}


	public CustomProgressBarRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == CustomProgressBarRenderer.class)
			mono.android.TypeManager.Activate ("ProjectOnlineMobile2.Android.Renderers.CustomProgressBarRenderer, ProjectOnlineMobile2.Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.Content.Context, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}

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
