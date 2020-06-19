using UnityEngine;
using System.Collections;

	public class AndroidEscape : MonoBehaviour
	{
		/// <summary>
		/// The is exist alert.
		/// </summary>
		public bool IsExistAlert{ get; set; }

		/// <summary>
		/// Update this instance.
		/// </summary>
		private void Update ()
		{
			// プラットフォームがアンドロイドかチェック
			if (Application.platform == RuntimePlatform.Android) {
				// エスケープキーのタッチアップを取得
				if (Input.GetKeyUp (KeyCode.Escape)) {
					//すでにアラートが表示されていたらリターン	
					if (IsExistAlert == true) {
						return;
					}
					IsExistAlert = true;
					AndroidJavaClass unity = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
					AndroidJavaObject activity = unity.GetStatic<AndroidJavaObject> ("currentActivity");
					activity.Call ("runOnUiThread", new AndroidJavaRunnable (() => {
						//ここでAlertDialogを作成
						AndroidJavaObject alertDialogBuilder = new AndroidJavaObject ("android.app.AlertDialog$Builder", activity);
						alertDialogBuilder.Call<AndroidJavaObject> ("setTitle", "確認");
						alertDialogBuilder.Call<AndroidJavaObject> ("setMessage", "アプリケーションを終了させますか？");
						alertDialogBuilder.Call<AndroidJavaObject> ("setCancelable", false);
						alertDialogBuilder.Call<AndroidJavaObject> ("setPositiveButton", "終了", new PositiveButtonListner (this));
						alertDialogBuilder.Call<AndroidJavaObject> ("setNegativeButton", "キャンセル", new NegativeButtonListner (this));
						AndroidJavaObject dialog = alertDialogBuilder.Call<AndroidJavaObject> ("create");
						dialog.Call ("show");
					}));
					return;
				}
			}
		}

		/// <summary>
		/// Positive button listner.
		/// </summary>
		private class PositiveButtonListner :AndroidJavaProxy
		{
			/// <summary>
			/// The parent.
			/// </summary>
			private AndroidEscape _parent;

			public PositiveButtonListner (AndroidEscape d) : base ("android.content.DialogInterface$OnClickListener")
			{
				//リスナーを作成した時に呼び出される
				_parent = d;
			}

			public void onClick (AndroidJavaObject obj, int value)
			{
				//ボタンが押された時に呼び出される
				// アプリケーション終了
				Application.Quit ();
				_parent.IsExistAlert = false;
			}
		}

		/// <summary>
		/// Negative button listner.
		/// </summary>
		private class NegativeButtonListner :AndroidJavaProxy
		{
			/// <summary>
			/// The parent.
			/// </summary>
			private AndroidEscape _parent;

			public NegativeButtonListner (AndroidEscape d) : base ("android.content.DialogInterface$OnClickListener")
			{
				//リスナーを作成した時に呼び出される
				_parent = d;
			}

			public void onClick (AndroidJavaObject obj, int value)
			{
				//ボタンが押された時に呼び出される
				_parent.IsExistAlert = false;
			}
		}
	}
