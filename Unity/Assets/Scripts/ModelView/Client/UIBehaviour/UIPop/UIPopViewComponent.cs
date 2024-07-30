using UnityEngine;
using UnityEngine.UI;
namespace ET.Client
{
	[ComponentOf(typeof(UIBaseWindow))]
	[EnableMethod]
	public  class UIPopViewComponent : Entity, IAwake, IDestroy 
	{
		public ESPopItem ESPopItem1
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_espopitem1 == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"PopItem/ESPopItem1");
		    	   this.m_espopitem1 = this.AddChild<ESPopItem,Transform>(subTrans);
     			}
     			return this.m_espopitem1;
     		}
     	}

		public ESPopItem ESPopItem2
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_espopitem2 == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"PopItem/ESPopItem2");
		    	   this.m_espopitem2 = this.AddChild<ESPopItem,Transform>(subTrans);
     			}
     			return this.m_espopitem2;
     		}
     	}

		public ESPopItem ESPopItem3
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_espopitem3 == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"PopItem/ESPopItem3");
		    	   this.m_espopitem3 = this.AddChild<ESPopItem,Transform>(subTrans);
     			}
     			return this.m_espopitem3;
     		}
     	}

		public ESPopItem ESPopItem4
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_espopitem4 == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"PopItem/ESPopItem4");
		    	   this.m_espopitem4 = this.AddChild<ESPopItem,Transform>(subTrans);
     			}
     			return this.m_espopitem4;
     		}
     	}

		public ESPopItem ESPopItem5
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_espopitem5 == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"PopItem/ESPopItem5");
		    	   this.m_espopitem5 = this.AddChild<ESPopItem,Transform>(subTrans);
     			}
     			return this.m_espopitem5;
     		}
     	}

		public ESPopItem ESPopItem6
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_espopitem6 == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"PopItem/ESPopItem6");
		    	   this.m_espopitem6 = this.AddChild<ESPopItem,Transform>(subTrans);
     			}
     			return this.m_espopitem6;
     		}
     	}

		public ESPopItem ESPopItem7
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_espopitem7 == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"PopItem/ESPopItem7");
		    	   this.m_espopitem7 = this.AddChild<ESPopItem,Transform>(subTrans);
     			}
     			return this.m_espopitem7;
     		}
     	}

		public ESPopItem ESPopItem8
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_espopitem8 == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"PopItem/ESPopItem8");
		    	   this.m_espopitem8 = this.AddChild<ESPopItem,Transform>(subTrans);
     			}
     			return this.m_espopitem8;
     		}
     	}

		public ESPopItem ESPopItem9
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_espopitem9 == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"PopItem/ESPopItem9");
		    	   this.m_espopitem9 = this.AddChild<ESPopItem,Transform>(subTrans);
     			}
     			return this.m_espopitem9;
     		}
     	}

		public ESPopItem ESPopItem10
     	{
     		get
     		{
     			if (this.uiTransform == null)
     			{
     				Log.Error("uiTransform is null.");
     				return null;
     			}
     			if( this.m_espopitem10 == null )
     			{
		    	   Transform subTrans = UIFindHelper.FindDeepChild<Transform>(this.uiTransform.gameObject,"PopItem/ESPopItem10");
		    	   this.m_espopitem10 = this.AddChild<ESPopItem,Transform>(subTrans);
     			}
     			return this.m_espopitem10;
     		}
     	}

		public void DestroyWidget()
		{
			this.m_espopitem1 = null;
			this.m_espopitem2 = null;
			this.m_espopitem3 = null;
			this.m_espopitem4 = null;
			this.m_espopitem5 = null;
			this.m_espopitem6 = null;
			this.m_espopitem7 = null;
			this.m_espopitem8 = null;
			this.m_espopitem9 = null;
			this.m_espopitem10 = null;
			uiTransform = null;
		}

		private EntityRef<ESPopItem> m_espopitem1 = null;
		private EntityRef<ESPopItem> m_espopitem2 = null;
		private EntityRef<ESPopItem> m_espopitem3 = null;
		private EntityRef<ESPopItem> m_espopitem4 = null;
		private EntityRef<ESPopItem> m_espopitem5 = null;
		private EntityRef<ESPopItem> m_espopitem6 = null;
		private EntityRef<ESPopItem> m_espopitem7 = null;
		private EntityRef<ESPopItem> m_espopitem8 = null;
		private EntityRef<ESPopItem> m_espopitem9 = null;
		private EntityRef<ESPopItem> m_espopitem10 = null;
		public Transform uiTransform = null;
	}
}
