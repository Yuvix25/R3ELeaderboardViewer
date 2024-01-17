; ModuleID = 'obj\Debug\130\android\marshal_methods.arm64-v8a.ll'
source_filename = "obj\Debug\130\android\marshal_methods.arm64-v8a.ll"
target datalayout = "e-m:e-i8:8:32-i16:16:32-i64:64-i128:128-n32:64-S128"
target triple = "aarch64-unknown-linux-android"


%struct.MonoImage = type opaque

%struct.MonoClass = type opaque

%struct.MarshalMethodsManagedClass = type {
	i32,; uint32_t token
	%struct.MonoClass*; MonoClass* klass
}

%struct.MarshalMethodName = type {
	i64,; uint64_t id
	i8*; char* name
}

%class._JNIEnv = type opaque

%class._jobject = type {
	i8; uint8_t b
}

%class._jclass = type {
	i8; uint8_t b
}

%class._jstring = type {
	i8; uint8_t b
}

%class._jthrowable = type {
	i8; uint8_t b
}

%class._jarray = type {
	i8; uint8_t b
}

%class._jobjectArray = type {
	i8; uint8_t b
}

%class._jbooleanArray = type {
	i8; uint8_t b
}

%class._jbyteArray = type {
	i8; uint8_t b
}

%class._jcharArray = type {
	i8; uint8_t b
}

%class._jshortArray = type {
	i8; uint8_t b
}

%class._jintArray = type {
	i8; uint8_t b
}

%class._jlongArray = type {
	i8; uint8_t b
}

%class._jfloatArray = type {
	i8; uint8_t b
}

%class._jdoubleArray = type {
	i8; uint8_t b
}

; assembly_image_cache
@assembly_image_cache = local_unnamed_addr global [0 x %struct.MonoImage*] zeroinitializer, align 8
; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = local_unnamed_addr constant [298 x i64] [
	i64 24362543149721218, ; 0: Xamarin.AndroidX.DynamicAnimation => 0x568d9a9a43a682 => 66
	i64 120698629574877762, ; 1: Mono.Android => 0x1accec39cafe242 => 4
	i64 210515253464952879, ; 2: Xamarin.AndroidX.Collection.dll => 0x2ebe681f694702f => 53
	i64 232391251801502327, ; 3: Xamarin.AndroidX.SavedState.dll => 0x3399e9cbc897277 => 87
	i64 233177144301842968, ; 4: Xamarin.AndroidX.Collection.Jvm.dll => 0x33c696097d9f218 => 54
	i64 295915112840604065, ; 5: Xamarin.AndroidX.SlidingPaneLayout => 0x41b4d3a3088a9a1 => 88
	i64 316157742385208084, ; 6: Xamarin.AndroidX.Core.Core.Ktx.dll => 0x46337caa7dc1b14 => 59
	i64 590536689428908136, ; 7: Xamarin.Android.Arch.Lifecycle.ViewModel.dll => 0x83201fd803ec868 => 21
	i64 634308326490598313, ; 8: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x8cd840fee8b6ba9 => 77
	i64 687654259221141486, ; 9: Xamarin.GooglePlayServices.Base => 0x98b09e7c92917ee => 123
	i64 702024105029695270, ; 10: System.Drawing.Common => 0x9be17343c0e7726 => 146
	i64 720058930071658100, ; 11: Xamarin.AndroidX.Legacy.Support.Core.UI => 0x9fe29c82844de74 => 71
	i64 816102801403336439, ; 12: Xamarin.Android.Support.Collections => 0xb53612c89d8d6f7 => 24
	i64 846634227898301275, ; 13: Xamarin.Android.Arch.Lifecycle.LiveData.Core => 0xbbfd9583890bb5b => 18
	i64 872800313462103108, ; 14: Xamarin.AndroidX.DrawerLayout => 0xc1ccf42c3c21c44 => 65
	i64 1000557547492888992, ; 15: Mono.Security.dll => 0xde2b1c9cba651a0 => 147
	i64 1120440138749646132, ; 16: Xamarin.Google.Android.Material.dll => 0xf8c9a5eae431534 => 112
	i64 1155807931551632357, ; 17: Xamarin.Io.PerfMark.PerfMarkApi.dll => 0x100a4130a4b6cbe5 => 134
	i64 1274338068859211160, ; 18: Xamarin.Grpc.Api => 0x11af5bb8ce1c4d98 => 128
	i64 1315114680217950157, ; 19: Xamarin.AndroidX.Arch.Core.Common.dll => 0x124039d5794ad7cd => 48
	i64 1342439039765371018, ; 20: Xamarin.Android.Support.Fragment => 0x12a14d31b1d4d88a => 33
	i64 1368633735297491523, ; 21: Xamarin.Firebase.Database.Collection.dll => 0x12fe5d218405e243 => 109
	i64 1392315331768750440, ; 22: Xamarin.Firebase.Auth.Interop.dll => 0x13527f6add681168 => 105
	i64 1465843056802068477, ; 23: Xamarin.Firebase.Components.dll => 0x1457b87e6928f7fd => 108
	i64 1474586420366808421, ; 24: Xamarin.Grpc.Android.dll => 0x1476c88960941565 => 127
	i64 1490981186906623721, ; 25: Xamarin.Android.Support.VersionedParcelable => 0x14b1077d6c5c0ee9 => 40
	i64 1576750169145655260, ; 26: Xamarin.AndroidX.Window.Extensions.Core.Core => 0x15e1bdecc376bfdc => 99
	i64 1624659445732251991, ; 27: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0x168bf32877da9957 => 46
	i64 1628611045998245443, ; 28: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0x1699fd1e1a00b643 => 79
	i64 1636321030536304333, ; 29: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0x16b5614ec39e16cd => 72
	i64 1795316252682057001, ; 30: Xamarin.AndroidX.AppCompat.dll => 0x18ea3e9eac997529 => 47
	i64 1836611346387731153, ; 31: Xamarin.AndroidX.SavedState => 0x197cf449ebe482d1 => 87
	i64 1875917498431009007, ; 32: Xamarin.AndroidX.Annotation.dll => 0x1a08990699eb70ef => 43
	i64 1956817255800234857, ; 33: Xamarin.Grpc.Android => 0x1b2802ed2e53e369 => 127
	i64 1981742497975770890, ; 34: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x1b80904d5c241f0a => 78
	i64 1990714127648872464, ; 35: Xamarin.Grpc.Core.dll => 0x1ba06ff3abdcd810 => 130
	i64 2064708342624596306, ; 36: Xamarin.Kotlin.StdLib.Jdk7.dll => 0x1ca7514c5eecb152 => 139
	i64 2136356949452311481, ; 37: Xamarin.AndroidX.MultiDex.dll => 0x1da5dd539d8acbb9 => 82
	i64 2165725771938924357, ; 38: Xamarin.AndroidX.Browser => 0x1e0e341d75540745 => 51
	i64 2203565783020068373, ; 39: Xamarin.KotlinX.Coroutines.Core => 0x1e94a367981dde15 => 142
	i64 2262844636196693701, ; 40: Xamarin.AndroidX.DrawerLayout.dll => 0x1f673d352266e6c5 => 65
	i64 2304837677853103545, ; 41: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 0x1ffc6da80d5ed5b9 => 86
	i64 2329709569556905518, ; 42: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x2054ca829b447e2e => 74
	i64 2343783402604882194, ; 43: Xamarin.Grpc.Stub.dll => 0x2086ca9636b86912 => 133
	i64 2470498323731680442, ; 44: Xamarin.AndroidX.CoordinatorLayout => 0x2248f922dc398cba => 58
	i64 2479423007379663237, ; 45: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x2268ae16b2cba985 => 93
	i64 2497223385847772520, ; 46: System.Runtime => 0x22a7eb7046413568 => 13
	i64 2547086958574651984, ; 47: Xamarin.AndroidX.Activity.dll => 0x2359121801df4a50 => 42
	i64 2592350477072141967, ; 48: System.Xml.dll => 0x23f9e10627330e8f => 14
	i64 2624866290265602282, ; 49: mscorlib.dll => 0x246d65fbde2db8ea => 5
	i64 2787234703088983483, ; 50: Xamarin.AndroidX.Startup.StartupRuntime => 0x26ae3f31ef429dbb => 89
	i64 2923871038697555247, ; 51: Jsr305Binding => 0x2893ad37e69ec52f => 3
	i64 2949706848458024531, ; 52: Xamarin.Android.Support.SlidingPaneLayout => 0x28ef76c01de0a653 => 38
	i64 2951672403965468947, ; 53: Xamarin.Firebase.Database.Collection => 0x28f67269abaf6113 => 109
	i64 2954987430423977617, ; 54: Xamarin.GooglePlayServices.Auth.Base => 0x290239696a2a5e91 => 121
	i64 2977248461349026546, ; 55: Xamarin.Android.Support.DrawerLayout => 0x29514fb392c97af2 => 32
	i64 3017704767998173186, ; 56: Xamarin.Google.Android.Material => 0x29e10a7f7d88a002 => 112
	i64 3070901203286954241, ; 57: Square.OkIO.JVM => 0x2a9e085fc23d1101 => 7
	i64 3171992396844006720, ; 58: Square.OkIO => 0x2c052e476c207d40 => 6
	i64 3179379704366398172, ; 59: R3ELeaderboardViewer.dll => 0x2c1f6cfefcd87adc => 0
	i64 3289520064315143713, ; 60: Xamarin.AndroidX.Lifecycle.Common => 0x2da6b911e3063621 => 73
	i64 3303437397778967116, ; 61: Xamarin.AndroidX.Annotation.Experimental => 0x2dd82acf985b2a4c => 44
	i64 3311221304742556517, ; 62: System.Numerics.Vectors.dll => 0x2df3d23ba9e2b365 => 12
	i64 3344514922410554693, ; 63: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x2e6a1a9a18463545 => 143
	i64 3364695309916733813, ; 64: Xamarin.Firebase.Common => 0x2eb1cc8eb5028175 => 106
	i64 3411255996856937470, ; 65: Xamarin.GooglePlayServices.Basement => 0x2f5737416a942bfe => 124
	i64 3427548605411023127, ; 66: Xamarin.GooglePlayServices.Auth.Api.Phone.dll => 0x2f91194bf3e8d917 => 120
	i64 3493805808809882663, ; 67: Xamarin.AndroidX.Tracing.Tracing.dll => 0x307c7ddf444f3427 => 91
	i64 3522470458906976663, ; 68: Xamarin.AndroidX.SwipeRefreshLayout => 0x30e2543832f52197 => 90
	i64 3531994851595924923, ; 69: System.Numerics => 0x31042a9aade235bb => 11
	i64 3571415421602489686, ; 70: System.Runtime.dll => 0x319037675df7e556 => 13
	i64 3727469159507183293, ; 71: Xamarin.AndroidX.RecyclerView => 0x33baa1739ba646bd => 85
	i64 3768479575991719956, ; 72: Xamarin.KotlinX.Coroutines.Play.Services.dll => 0x344c5435464d1814 => 144
	i64 3772598417116884899, ; 73: Xamarin.AndroidX.DynamicAnimation.dll => 0x345af645b473efa3 => 66
	i64 4045730230152541805, ; 74: Xamarin.Grpc.Protobuf.Lite.dll => 0x38255235894d366d => 132
	i64 4201423742386704971, ; 75: Xamarin.AndroidX.Core.Core.Ktx => 0x3a4e74a233da124b => 59
	i64 4247996603072512073, ; 76: Xamarin.GooglePlayServices.Tasks => 0x3af3ea6755340049 => 126
	i64 4252163538099460320, ; 77: Xamarin.Android.Support.ViewPager.dll => 0x3b02b8357f4958e0 => 41
	i64 4311005257343236288, ; 78: Xamarin.GooglePlayServices.Fido => 0x3bd3c470dcc8f8c0 => 125
	i64 4349341163892612332, ; 79: Xamarin.Android.Support.DocumentFile => 0x3c5bf6bea8cd9cec => 31
	i64 4416987920449902723, ; 80: Xamarin.Android.Support.AsyncLayoutInflater.dll => 0x3d4c4b1c879b9883 => 23
	i64 4636684751163556186, ; 81: Xamarin.AndroidX.VersionedParcelable.dll => 0x4058d0370893015a => 95
	i64 4702770163853758138, ; 82: Xamarin.Firebase.Components => 0x4143988c34cf0eba => 108
	i64 4759461199762736555, ; 83: Xamarin.AndroidX.Lifecycle.Process.dll => 0x420d00be961cc5ab => 76
	i64 4782108999019072045, ; 84: Xamarin.AndroidX.AsyncLayoutInflater.dll => 0x425d76cc43bb0a2d => 50
	i64 4794310189461587505, ; 85: Xamarin.AndroidX.Activity => 0x4288cfb749e4c631 => 42
	i64 4795410492532947900, ; 86: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0x428cb86f8f9b7bbc => 90
	i64 4841234827713643511, ; 87: Xamarin.Android.Support.CoordinaterLayout => 0x432f856d041f03f7 => 26
	i64 4963205065368577818, ; 88: Xamarin.Android.Support.LocalBroadcastManager.dll => 0x44e0d8b5f4b6a71a => 36
	i64 5178572682164047940, ; 89: Xamarin.Android.Support.Print.dll => 0x47ddfc6acbee1044 => 37
	i64 5203618020066742981, ; 90: Xamarin.Essentials => 0x4836f704f0e652c5 => 101
	i64 5205316157927637098, ; 91: Xamarin.AndroidX.LocalBroadcastManager => 0x483cff7778e0c06a => 81
	i64 5258427006098912452, ; 92: Xamarin.GooglePlayServices.Auth.Base.dll => 0x48f9af806fd8b4c4 => 121
	i64 5288341611614403055, ; 93: Xamarin.Android.Support.Interpolator.dll => 0x4963f6ad4b3179ef => 34
	i64 5290215063822704973, ; 94: Xamarin.Grpc.Stub => 0x496a9e926092a14d => 133
	i64 5376510917114486089, ; 95: Xamarin.AndroidX.VectorDrawable.Animated => 0x4a9d3431719e5d49 => 93
	i64 5408338804355907810, ; 96: Xamarin.AndroidX.Transition => 0x4b0e477cea9840e2 => 92
	i64 5451019430259338467, ; 97: Xamarin.AndroidX.ConstraintLayout.dll => 0x4ba5e94a845c2ce3 => 57
	i64 5507995362134886206, ; 98: System.Core.dll => 0x4c705499688c873e => 8
	i64 5574231584441077149, ; 99: Xamarin.AndroidX.Annotation.Jvm => 0x4d5ba617ae5f8d9d => 45
	i64 5692067934154308417, ; 100: Xamarin.AndroidX.ViewPager2.dll => 0x4efe49a0d4a8bb41 => 97
	i64 5757522595884336624, ; 101: Xamarin.AndroidX.Concurrent.Futures.dll => 0x4fe6d44bd9f885f0 => 55
	i64 5767696078500135884, ; 102: Xamarin.Android.Support.Annotations.dll => 0x500af9065b6a03cc => 22
	i64 5896680224035167651, ; 103: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x51d5376bfbafdda3 => 75
	i64 6044705416426755068, ; 104: Xamarin.Android.Support.SwipeRefreshLayout.dll => 0x53e31b8ccdff13fc => 39
	i64 6058153446002397952, ; 105: Xamarin.Firebase.Common.Ktx => 0x5412e2762fc81300 => 107
	i64 6118452257458269359, ; 106: Xamarin.Firebase.AppCheck.Interop.dll => 0x54e91be944fcacaf => 103
	i64 6135981624229292808, ; 107: Xamarin.Grpc.Api.dll => 0x552762c70482eb08 => 128
	i64 6311200438583329442, ; 108: Xamarin.Android.Support.LocalBroadcastManager => 0x5795e35c580c82a2 => 36
	i64 6319713645133255417, ; 109: Xamarin.AndroidX.Lifecycle.Runtime => 0x57b42213b45b52f9 => 77
	i64 6401242110442382339, ; 110: Xamarin.Protobuf.JavaLite => 0x58d5c7c8c230a403 => 145
	i64 6401687960814735282, ; 111: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0x58d75d486341cfb2 => 74
	i64 6403742896930319886, ; 112: Xamarin.Firebase.Auth.dll => 0x58deaa3c7c766e0e => 104
	i64 6504860066809920875, ; 113: Xamarin.AndroidX.Browser.dll => 0x5a45e7c43bd43d6b => 51
	i64 6548213210057960872, ; 114: Xamarin.AndroidX.CustomView.dll => 0x5adfed387b066da8 => 62
	i64 6557084851308642443, ; 115: Xamarin.AndroidX.Window.dll => 0x5aff71ee6c58c08b => 98
	i64 6589202984700901502, ; 116: Xamarin.Google.ErrorProne.Annotations.dll => 0x5b718d34180a787e => 115
	i64 6657448669945361351, ; 117: Xamarin.Google.Android.Play.Integrity => 0x5c64024aea7d73c7 => 113
	i64 6659513131007730089, ; 118: Xamarin.AndroidX.Legacy.Support.Core.UI.dll => 0x5c6b57e8b6c3e1a9 => 71
	i64 6894844156784520562, ; 119: System.Numerics.Vectors => 0x5faf683aead1ad72 => 12
	i64 6975328107116786489, ; 120: Xamarin.Firebase.Annotations => 0x60cd57f4e07e7339 => 102
	i64 7103753931438454322, ; 121: Xamarin.AndroidX.Interpolator.dll => 0x62959a90372c7632 => 70
	i64 7152933704405506614, ; 122: Xamarin.Google.Android.Play.Integrity.dll => 0x6344534e69025a36 => 113
	i64 7194160955514091247, ; 123: Xamarin.Android.Support.CursorAdapter.dll => 0x63d6cb45d266f6ef => 29
	i64 7637365915383206639, ; 124: Xamarin.Essentials.dll => 0x69fd5fd5e61792ef => 101
	i64 7654504624184590948, ; 125: System.Net.Http => 0x6a3a4366801b8264 => 10
	i64 7735352534559001595, ; 126: Xamarin.Kotlin.StdLib.dll => 0x6b597e2582ce8bfb => 138
	i64 7821246742157274664, ; 127: Xamarin.Android.Support.AsyncLayoutInflater => 0x6c8aa67926f72e28 => 23
	i64 7836164640616011524, ; 128: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x6cbfa6390d64d704 => 46
	i64 7991572870742010042, ; 129: Xamarin.Firebase.Firestore.dll => 0x6ee7c52f4d39e8ba => 110
	i64 8044118961405839122, ; 130: System.ComponentModel.Composition => 0x6fa2739369944712 => 148
	i64 8083354569033831015, ; 131: Xamarin.AndroidX.Lifecycle.Common.dll => 0x702dd82730cad267 => 73
	i64 8101777744205214367, ; 132: Xamarin.Android.Support.Annotations => 0x706f4beeec84729f => 22
	i64 8167236081217502503, ; 133: Java.Interop.dll => 0x7157d9f1a9b8fd27 => 2
	i64 8187640529827139739, ; 134: Xamarin.KotlinX.Coroutines.Android => 0x71a057ae90f0109b => 141
	i64 8196541262927413903, ; 135: Xamarin.Android.Support.Interpolator => 0x71bff6d9fb9ec28f => 34
	i64 8385935383968044654, ; 136: Xamarin.Android.Arch.Lifecycle.Runtime.dll => 0x7460d3cd16cb566e => 20
	i64 8398329775253868912, ; 137: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x748cdc6f3097d170 => 56
	i64 8426919725312979251, ; 138: Xamarin.AndroidX.Lifecycle.Process => 0x74f26ed7aa033133 => 76
	i64 8598790081731763592, ; 139: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 0x77550a055fc61d88 => 68
	i64 8601935802264776013, ; 140: Xamarin.AndroidX.Transition.dll => 0x7760370982b4ed4d => 92
	i64 8605236455805933405, ; 141: Xamarin.Google.Android.Recaptcha.dll => 0x776bf0f6cc8dd75d => 114
	i64 8609060182490045521, ; 142: Square.OkIO.dll => 0x7779869f8b475c51 => 6
	i64 8613760304861496997, ; 143: Xamarin.CodeHaus.Mojo.AnimalSnifferAnnotations.dll => 0x778a395c0fa146a5 => 100
	i64 8626175481042262068, ; 144: Java.Interop => 0x77b654e585b55834 => 2
	i64 8808820144457481518, ; 145: Xamarin.Android.Support.Loader.dll => 0x7a3f374010b17d2e => 35
	i64 8853378295825400934, ; 146: Xamarin.Kotlin.StdLib.Common.dll => 0x7add84a720d38466 => 137
	i64 8917102979740339192, ; 147: Xamarin.Android.Support.DocumentFile.dll => 0x7bbfe9ea4d000bf8 => 31
	i64 8951477988056063522, ; 148: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 0x7c3a09cd9ccf5e22 => 84
	i64 9031035476476434958, ; 149: Xamarin.KotlinX.Coroutines.Core.dll => 0x7d54aeead9541a0e => 142
	i64 9312692141327339315, ; 150: Xamarin.AndroidX.ViewPager2 => 0x813d54296a634f33 => 97
	i64 9324707631942237306, ; 151: Xamarin.AndroidX.AppCompat => 0x8168042fd44a7c7a => 47
	i64 9490522350195345034, ; 152: Xamarin.Google.Android.Recaptcha => 0x83b51bcb684c868a => 114
	i64 9662334977499516867, ; 153: System.Numerics.dll => 0x8617827802b0cfc3 => 11
	i64 9678050649315576968, ; 154: Xamarin.AndroidX.CoordinatorLayout.dll => 0x864f57c9feb18c88 => 58
	i64 9780093022148426479, ; 155: Xamarin.AndroidX.Window.Extensions.Core.Core.dll => 0x87b9dec9576efaef => 99
	i64 9808709177481450983, ; 156: Mono.Android.dll => 0x881f890734e555e7 => 4
	i64 9825649861376906464, ; 157: Xamarin.AndroidX.Concurrent.Futures => 0x885bb87d8abc94e0 => 55
	i64 9866412715007501892, ; 158: Xamarin.Android.Arch.Lifecycle.Common.dll => 0x88ec8a16fd6b6644 => 17
	i64 9875200773399460291, ; 159: Xamarin.GooglePlayServices.Base.dll => 0x890bc2c8482339c3 => 123
	i64 9907349773706910547, ; 160: Xamarin.AndroidX.Emoji2.ViewsHelper => 0x897dfa20b758db53 => 68
	i64 9998632235833408227, ; 161: Mono.Security => 0x8ac2470b209ebae3 => 147
	i64 10038780035334861115, ; 162: System.Net.Http.dll => 0x8b50e941206af13b => 10
	i64 10071983904436292605, ; 163: Xamarin.CodeHaus.Mojo.AnimalSnifferAnnotations => 0x8bc6dfff57608bfd => 100
	i64 10167561595017141208, ; 164: Xamarin.GoogleAndroid.Annotations.dll => 0x8d1a6f668ee69bd8 => 119
	i64 10226222362177979215, ; 165: Xamarin.Kotlin.StdLib.Jdk7 => 0x8dead70ebbc6434f => 139
	i64 10229024438826829339, ; 166: Xamarin.AndroidX.CustomView => 0x8df4cb880b10061b => 62
	i64 10303855825347935641, ; 167: Xamarin.Android.Support.Loader => 0x8efea647eeb3fd99 => 35
	i64 10321854143672141184, ; 168: Xamarin.Jetbrains.Annotations.dll => 0x8f3e97a7f8f8c580 => 136
	i64 10363495123250631811, ; 169: Xamarin.Android.Support.Collections.dll => 0x8fd287e80cd8d483 => 24
	i64 10376576884623852283, ; 170: Xamarin.AndroidX.Tracing.Tracing => 0x900101b2f888c2fb => 91
	i64 10391565382270571827, ; 171: R3ELeaderboardViewer => 0x903641a8867bd533 => 0
	i64 10406448008575299332, ; 172: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x906b2153fcb3af04 => 143
	i64 10430153318873392755, ; 173: Xamarin.AndroidX.Core => 0x90bf592ea44f6673 => 60
	i64 10635644668885628703, ; 174: Xamarin.Android.Support.DrawerLayout.dll => 0x93996679ee34771f => 32
	i64 10847732767863316357, ; 175: Xamarin.AndroidX.Arch.Core.Common => 0x968ae37a86db9f85 => 48
	i64 10850923258212604222, ; 176: Xamarin.Android.Arch.Lifecycle.Runtime => 0x9696393672c9593e => 20
	i64 10857315922431607327, ; 177: Xamarin.Firebase.ProtoliteWellKnownTypes => 0x96acef4e92ba821f => 111
	i64 10966933586012635777, ; 178: Xamarin.Grpc.OkHttp.dll => 0x98325ffdbd958281 => 131
	i64 11019817191295005410, ; 179: Xamarin.AndroidX.Annotation.Jvm.dll => 0x98ee415998e1b2e2 => 45
	i64 11023048688141570732, ; 180: System.Core => 0x98f9bc61168392ac => 8
	i64 11037814507248023548, ; 181: System.Xml => 0x992e31d0412bf7fc => 14
	i64 11071824625609515081, ; 182: Xamarin.Google.ErrorProne.Annotations => 0x99a705d600e0a049 => 115
	i64 11072526564452562534, ; 183: Square.OkIO.JVM.dll => 0x99a9843ee0457666 => 7
	i64 11136029745144976707, ; 184: Jsr305Binding.dll => 0x9a8b200d4f8cd543 => 3
	i64 11162124722117608902, ; 185: Xamarin.AndroidX.ViewPager => 0x9ae7d54b986d05c6 => 96
	i64 11299661109949763898, ; 186: Xamarin.AndroidX.Collection.Jvm => 0x9cd075e94cda113a => 54
	i64 11340910727871153756, ; 187: Xamarin.AndroidX.CursorAdapter => 0x9d630238642d465c => 61
	i64 11361951459766490322, ; 188: Xamarin.GoogleAndroid.Annotations => 0x9dadc2a78a9cd4d2 => 119
	i64 11376351552967644903, ; 189: Xamarin.Firebase.Annotations.dll => 0x9de0eb76829996e7 => 102
	i64 11376461258732682436, ; 190: Xamarin.Android.Support.Compat => 0x9de14f3d5fc13cc4 => 25
	i64 11392833485892708388, ; 191: Xamarin.AndroidX.Print.dll => 0x9e1b79b18fcf6824 => 83
	i64 11496466075493495264, ; 192: Xamarin.Grpc.Context.dll => 0x9f8ba6fc1a1e71e0 => 129
	i64 11529969570048099689, ; 193: Xamarin.AndroidX.ViewPager.dll => 0xa002ae3c4dc7c569 => 96
	i64 11543422055205009205, ; 194: Xamarin.Firebase.Firestore => 0xa032793314e77735 => 110
	i64 11580057168383206117, ; 195: Xamarin.AndroidX.Annotation => 0xa0b4a0a4103262e5 => 43
	i64 11591352189662810718, ; 196: Xamarin.AndroidX.Startup.StartupRuntime.dll => 0xa0dcc167234c525e => 89
	i64 11672361001936329215, ; 197: Xamarin.AndroidX.Interpolator => 0xa1fc8e7d0a8999ff => 70
	i64 11834399401546345650, ; 198: Xamarin.Android.Support.SlidingPaneLayout.dll => 0xa43c3b8deb43ecb2 => 38
	i64 11864794479965678424, ; 199: Xamarin.Protobuf.JavaLite.dll => 0xa4a837b7975eab58 => 145
	i64 11865714326292153359, ; 200: Xamarin.Android.Arch.Lifecycle.LiveData => 0xa4ab7c5000e8440f => 19
	i64 11890931220905723078, ; 201: Xamarin.GooglePlayServices.Fido.dll => 0xa50512f1ceb004c6 => 125
	i64 12013962889899020729, ; 202: Xamarin.GooglePlayServices.Auth => 0xa6ba2b987d2811b9 => 122
	i64 12137774235383566651, ; 203: Xamarin.AndroidX.VectorDrawable => 0xa872095bbfed113b => 94
	i64 12336928085371509187, ; 204: Xamarin.GooglePlayServices.Auth.Api.Phone => 0xab3592bad41bd9c3 => 120
	i64 12346958216201575315, ; 205: Xamarin.JavaX.Inject.dll => 0xab593514a5491b93 => 135
	i64 12388767885335911387, ; 206: Xamarin.Android.Arch.Lifecycle.LiveData.dll => 0xabedbec0d236dbdb => 19
	i64 12414299427252656003, ; 207: Xamarin.Android.Support.Compat.dll => 0xac48738e28bad783 => 25
	i64 12451044538927396471, ; 208: Xamarin.AndroidX.Fragment.dll => 0xaccaff0a2955b677 => 69
	i64 12466513435562512481, ; 209: Xamarin.AndroidX.Loader.dll => 0xad01f3eb52569061 => 80
	i64 12487638416075308985, ; 210: Xamarin.AndroidX.DocumentFile.dll => 0xad4d00fa21b0bfb9 => 64
	i64 12538491095302438457, ; 211: Xamarin.AndroidX.CardView.dll => 0xae01ab382ae67e39 => 52
	i64 12700543734426720211, ; 212: Xamarin.AndroidX.Collection => 0xb041653c70d157d3 => 53
	i64 12753841065332862057, ; 213: Xamarin.AndroidX.Window => 0xb0febee04cf46c69 => 98
	i64 12828192437253469131, ; 214: Xamarin.Kotlin.StdLib.Jdk8.dll => 0xb206e50e14d873cb => 140
	i64 12952608645614506925, ; 215: Xamarin.Android.Support.Core.Utils => 0xb3c0e8eff48193ad => 28
	i64 12963446364377008305, ; 216: System.Drawing.Common.dll => 0xb3e769c8fd8548b1 => 146
	i64 12982280885948128408, ; 217: Xamarin.AndroidX.CustomView.PoolingContainer => 0xb42a53aec5481c98 => 63
	i64 13084382143907087733, ; 218: Xamarin.Grpc.Context => 0xb595103c610bc575 => 129
	i64 13129914918964716986, ; 219: Xamarin.AndroidX.Emoji2.dll => 0xb636d40db3fe65ba => 67
	i64 13358059602087096138, ; 220: Xamarin.Android.Support.Fragment.dll => 0xb9615c6f1ee5af4a => 33
	i64 13401370062847626945, ; 221: Xamarin.AndroidX.VectorDrawable.dll => 0xb9fb3b1193964ec1 => 94
	i64 13402939433517888790, ; 222: Xamarin.Google.Guava.FailureAccess => 0xba00ce6728e8b516 => 117
	i64 13404347523447273790, ; 223: Xamarin.AndroidX.ConstraintLayout.Core => 0xba05cf0da4f6393e => 56
	i64 13454009404024712428, ; 224: Xamarin.Google.Guava.ListenableFuture => 0xbab63e4543a86cec => 118
	i64 13465488254036897740, ; 225: Xamarin.Kotlin.StdLib => 0xbadf06394d106fcc => 138
	i64 13491513212026656886, ; 226: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0xbb3b7bc905569876 => 49
	i64 13572454107664307259, ; 227: Xamarin.AndroidX.RecyclerView.dll => 0xbc5b0b19d99f543b => 85
	i64 13609095008681508810, ; 228: Xamarin.Grpc.Protobuf.Lite => 0xbcdd37ce6b00bfca => 132
	i64 13621154251410165619, ; 229: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 0xbd080f9faa1acf73 => 63
	i64 13807020823704499900, ; 230: Xamarin.Firebase.Common.Ktx.dll => 0xbf9c64495353f6bc => 107
	i64 13828521679616088467, ; 231: Xamarin.Kotlin.StdLib.Common => 0xbfe8c733724e1993 => 137
	i64 13865727802090930648, ; 232: Xamarin.Google.Guava.dll => 0xc06cf5f8e3e341d8 => 116
	i64 13959074834287824816, ; 233: Xamarin.AndroidX.Fragment => 0xc1b8989a7ad20fb0 => 69
	i64 13975254687929967048, ; 234: Xamarin.Google.Guava => 0xc1f2141837ada1c8 => 116
	i64 14124974489674258913, ; 235: Xamarin.AndroidX.CardView => 0xc405fd76067d19e1 => 52
	i64 14165531176311179688, ; 236: Xamarin.Firebase.Auth => 0xc496138d7abfc9a8 => 104
	i64 14172845254133543601, ; 237: Xamarin.AndroidX.MultiDex => 0xc4b00faaed35f2b1 => 82
	i64 14261073672896646636, ; 238: Xamarin.AndroidX.Print => 0xc5e982f274ae0dec => 83
	i64 14382082037123372364, ; 239: Xamarin.Firebase.Auth.Interop => 0xc7976b69c943d54c => 105
	i64 14400856865250966808, ; 240: Xamarin.Android.Support.Core.UI => 0xc7da1f051a877d18 => 27
	i64 14486659737292545672, ; 241: Xamarin.AndroidX.Lifecycle.LiveData => 0xc90af44707469e88 => 75
	i64 14495724990987328804, ; 242: Xamarin.AndroidX.ResourceInspection.Annotation => 0xc92b2913e18d5d24 => 86
	i64 14524915121004231475, ; 243: Xamarin.JavaX.Inject => 0xc992dd58a4283b33 => 135
	i64 14644440854989303794, ; 244: Xamarin.AndroidX.LocalBroadcastManager.dll => 0xcb3b815e37daeff2 => 81
	i64 14661790646341542033, ; 245: Xamarin.Android.Support.SwipeRefreshLayout => 0xcb7924e94e552091 => 39
	i64 14671188939680189912, ; 246: Xamarin.Grpc.Core => 0xcb9a889bfe470dd8 => 130
	i64 14698606024688292729, ; 247: Xamarin.Io.PerfMark.PerfMarkApi => 0xcbfbf04d8af65379 => 134
	i64 14789919016435397935, ; 248: Xamarin.Firebase.Common.dll => 0xcd4058fc2f6d352f => 106
	i64 14792063746108907174, ; 249: Xamarin.Google.Guava.ListenableFuture.dll => 0xcd47f79af9c15ea6 => 118
	i64 14852515768018889994, ; 250: Xamarin.AndroidX.CursorAdapter.dll => 0xce1ebc6625a76d0a => 61
	i64 14889905118082851278, ; 251: GoogleGson.dll => 0xcea391d0969961ce => 1
	i64 14988210264188246988, ; 252: Xamarin.AndroidX.DocumentFile => 0xd000d1d307cddbcc => 64
	i64 15099396616243600100, ; 253: Xamarin.KotlinX.Coroutines.Play.Services => 0xd18bd538f1ef5ae4 => 144
	i64 15150743910298169673, ; 254: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 0xd2424150783c3149 => 84
	i64 15188640517174936311, ; 255: Xamarin.Android.Arch.Core.Common => 0xd2c8e413d75142f7 => 15
	i64 15246441518555807158, ; 256: Xamarin.Android.Arch.Core.Common.dll => 0xd3963dc832493db6 => 15
	i64 15279429628684179188, ; 257: Xamarin.KotlinX.Coroutines.Android.dll => 0xd40b704b1c4c96f4 => 141
	i64 15326820765897713587, ; 258: Xamarin.Android.Arch.Core.Runtime.dll => 0xd4b3ce481769e7b3 => 16
	i64 15370334346939861994, ; 259: Xamarin.AndroidX.Core.dll => 0xd54e65a72c560bea => 60
	i64 15568534730848034786, ; 260: Xamarin.Android.Support.VersionedParcelable.dll => 0xd80e8bda21875fe2 => 40
	i64 15582737692548360875, ; 261: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xd841015ed86f6aab => 79
	i64 15609085926864131306, ; 262: System.dll => 0xd89e9cf3334914ea => 9
	i64 15777549416145007739, ; 263: Xamarin.AndroidX.SlidingPaneLayout.dll => 0xdaf51d99d77eb47b => 88
	i64 15788897513097211459, ; 264: Xamarin.Firebase.ProtoliteWellKnownTypes.dll => 0xdb1d6ea28f352e43 => 111
	i64 15930129725311349754, ; 265: Xamarin.GooglePlayServices.Tasks.dll => 0xdd1330956f12f3fa => 126
	i64 16154507427712707110, ; 266: System => 0xe03056ea4e39aa26 => 9
	i64 16242842420508142678, ; 267: Xamarin.Android.Support.CoordinaterLayout.dll => 0xe16a2b1f8908ac56 => 26
	i64 16303230644352379770, ; 268: Xamarin.Grpc.OkHttp => 0xe240b5e48fe2eb7a => 131
	i64 16423015068819898779, ; 269: Xamarin.Kotlin.StdLib.Jdk8 => 0xe3ea453135e5c19b => 140
	i64 16565028646146589191, ; 270: System.ComponentModel.Composition.dll => 0xe5e2cdc9d3bcc207 => 148
	i64 16579050217386591297, ; 271: Xamarin.Google.Guava.FailureAccess.dll => 0xe6149e5548b0c441 => 117
	i64 16621146507174665210, ; 272: Xamarin.AndroidX.ConstraintLayout => 0xe6aa2caf87dedbfa => 57
	i64 16767985610513713374, ; 273: Xamarin.Android.Arch.Core.Runtime => 0xe8b3da12798808de => 16
	i64 16833383113903931215, ; 274: mscorlib => 0xe99c30c1484d7f4f => 5
	i64 16932527889823454152, ; 275: Xamarin.Android.Support.Core.Utils.dll => 0xeafc6c67465253c8 => 28
	i64 17024911836938395553, ; 276: Xamarin.AndroidX.Annotation.Experimental.dll => 0xec44a31d250e5fa1 => 44
	i64 17037200463775726619, ; 277: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xec704b8e0a78fc1b => 72
	i64 17310799966561153083, ; 278: Xamarin.GooglePlayServices.Auth.dll => 0xf03c50da60b8b03b => 122
	i64 17383232329670771222, ; 279: Xamarin.Android.Support.CustomView.dll => 0xf13da5b41a1cc216 => 30
	i64 17428701562824544279, ; 280: Xamarin.Android.Support.Core.UI.dll => 0xf1df2fbaec73d017 => 27
	i64 17483646997724851973, ; 281: Xamarin.Android.Support.ViewPager => 0xf2a2644fe5b6ef05 => 41
	i64 17522591619082469157, ; 282: GoogleGson => 0xf32cc03d27a5bf25 => 1
	i64 17524135665394030571, ; 283: Xamarin.Android.Support.Print => 0xf3323c8a739097eb => 37
	i64 17544493274320527064, ; 284: Xamarin.AndroidX.AsyncLayoutInflater => 0xf37a8fada41aded8 => 50
	i64 17605100189928655442, ; 285: Xamarin.Firebase.AppCheck.Interop => 0xf451e158cfdc0a52 => 103
	i64 17666959971718154066, ; 286: Xamarin.Android.Support.CustomView => 0xf52da67d9f4e4752 => 30
	i64 17704177640604968747, ; 287: Xamarin.AndroidX.Loader => 0xf5b1dfc36cac272b => 80
	i64 17710060891934109755, ; 288: Xamarin.AndroidX.Lifecycle.ViewModel => 0xf5c6c68c9e45303b => 78
	i64 17760961058993581169, ; 289: Xamarin.Android.Arch.Lifecycle.Common => 0xf67b9bfb46dbac71 => 17
	i64 17841643939744178149, ; 290: Xamarin.Android.Arch.Lifecycle.ViewModel => 0xf79a40a25573dfe5 => 21
	i64 17891337867145587222, ; 291: Xamarin.Jetbrains.Annotations => 0xf84accff6fb52a16 => 136
	i64 17958105683855786126, ; 292: Xamarin.Android.Arch.Lifecycle.LiveData.Core.dll => 0xf93801f92d25c08e => 18
	i64 17986907704309214542, ; 293: Xamarin.GooglePlayServices.Basement.dll => 0xf99e554223166d4e => 124
	i64 18116111925905154859, ; 294: Xamarin.AndroidX.Arch.Core.Runtime => 0xfb695bd036cb632b => 49
	i64 18260797123374478311, ; 295: Xamarin.AndroidX.Emoji2 => 0xfd6b623bde35f3e7 => 67
	i64 18301997741680159453, ; 296: Xamarin.Android.Support.CursorAdapter => 0xfdfdc1fa58d8eadd => 29
	i64 18380184030268848184 ; 297: Xamarin.AndroidX.VersionedParcelable => 0xff1387fe3e7b7838 => 95
], align 8
@assembly_image_cache_indices = local_unnamed_addr constant [298 x i32] [
	i32 66, i32 4, i32 53, i32 87, i32 54, i32 88, i32 59, i32 21, ; 0..7
	i32 77, i32 123, i32 146, i32 71, i32 24, i32 18, i32 65, i32 147, ; 8..15
	i32 112, i32 134, i32 128, i32 48, i32 33, i32 109, i32 105, i32 108, ; 16..23
	i32 127, i32 40, i32 99, i32 46, i32 79, i32 72, i32 47, i32 87, ; 24..31
	i32 43, i32 127, i32 78, i32 130, i32 139, i32 82, i32 51, i32 142, ; 32..39
	i32 65, i32 86, i32 74, i32 133, i32 58, i32 93, i32 13, i32 42, ; 40..47
	i32 14, i32 5, i32 89, i32 3, i32 38, i32 109, i32 121, i32 32, ; 48..55
	i32 112, i32 7, i32 6, i32 0, i32 73, i32 44, i32 12, i32 143, ; 56..63
	i32 106, i32 124, i32 120, i32 91, i32 90, i32 11, i32 13, i32 85, ; 64..71
	i32 144, i32 66, i32 132, i32 59, i32 126, i32 41, i32 125, i32 31, ; 72..79
	i32 23, i32 95, i32 108, i32 76, i32 50, i32 42, i32 90, i32 26, ; 80..87
	i32 36, i32 37, i32 101, i32 81, i32 121, i32 34, i32 133, i32 93, ; 88..95
	i32 92, i32 57, i32 8, i32 45, i32 97, i32 55, i32 22, i32 75, ; 96..103
	i32 39, i32 107, i32 103, i32 128, i32 36, i32 77, i32 145, i32 74, ; 104..111
	i32 104, i32 51, i32 62, i32 98, i32 115, i32 113, i32 71, i32 12, ; 112..119
	i32 102, i32 70, i32 113, i32 29, i32 101, i32 10, i32 138, i32 23, ; 120..127
	i32 46, i32 110, i32 148, i32 73, i32 22, i32 2, i32 141, i32 34, ; 128..135
	i32 20, i32 56, i32 76, i32 68, i32 92, i32 114, i32 6, i32 100, ; 136..143
	i32 2, i32 35, i32 137, i32 31, i32 84, i32 142, i32 97, i32 47, ; 144..151
	i32 114, i32 11, i32 58, i32 99, i32 4, i32 55, i32 17, i32 123, ; 152..159
	i32 68, i32 147, i32 10, i32 100, i32 119, i32 139, i32 62, i32 35, ; 160..167
	i32 136, i32 24, i32 91, i32 0, i32 143, i32 60, i32 32, i32 48, ; 168..175
	i32 20, i32 111, i32 131, i32 45, i32 8, i32 14, i32 115, i32 7, ; 176..183
	i32 3, i32 96, i32 54, i32 61, i32 119, i32 102, i32 25, i32 83, ; 184..191
	i32 129, i32 96, i32 110, i32 43, i32 89, i32 70, i32 38, i32 145, ; 192..199
	i32 19, i32 125, i32 122, i32 94, i32 120, i32 135, i32 19, i32 25, ; 200..207
	i32 69, i32 80, i32 64, i32 52, i32 53, i32 98, i32 140, i32 28, ; 208..215
	i32 146, i32 63, i32 129, i32 67, i32 33, i32 94, i32 117, i32 56, ; 216..223
	i32 118, i32 138, i32 49, i32 85, i32 132, i32 63, i32 107, i32 137, ; 224..231
	i32 116, i32 69, i32 116, i32 52, i32 104, i32 82, i32 83, i32 105, ; 232..239
	i32 27, i32 75, i32 86, i32 135, i32 81, i32 39, i32 130, i32 134, ; 240..247
	i32 106, i32 118, i32 61, i32 1, i32 64, i32 144, i32 84, i32 15, ; 248..255
	i32 15, i32 141, i32 16, i32 60, i32 40, i32 79, i32 9, i32 88, ; 256..263
	i32 111, i32 126, i32 9, i32 26, i32 131, i32 140, i32 148, i32 117, ; 264..271
	i32 57, i32 16, i32 5, i32 28, i32 44, i32 72, i32 122, i32 30, ; 272..279
	i32 27, i32 41, i32 1, i32 37, i32 50, i32 103, i32 30, i32 80, ; 280..287
	i32 78, i32 17, i32 21, i32 136, i32 18, i32 124, i32 49, i32 67, ; 288..295
	i32 29, i32 95 ; 296..297
], align 4

@marshal_methods_number_of_classes = local_unnamed_addr constant i32 0, align 4

; marshal_methods_class_cache
@marshal_methods_class_cache = global [0 x %struct.MarshalMethodsManagedClass] [
], align 8; end of 'marshal_methods_class_cache' array


@get_function_pointer = internal unnamed_addr global void (i32, i32, i32, i8**)* null, align 8

; Function attributes: "frame-pointer"="non-leaf" "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+neon,+outline-atomics" uwtable willreturn writeonly
define void @xamarin_app_init (void (i32, i32, i32, i8**)* %fn) local_unnamed_addr #0
{
	store void (i32, i32, i32, i8**)* %fn, void (i32, i32, i32, i8**)** @get_function_pointer, align 8
	ret void
}

; Names of classes in which marshal methods reside
@mm_class_names = local_unnamed_addr constant [0 x i8*] zeroinitializer, align 8
@__MarshalMethodName_name.0 = internal constant [1 x i8] c"\00", align 1

; mm_method_names
@mm_method_names = local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	; 0
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		i8* getelementptr inbounds ([1 x i8], [1 x i8]* @__MarshalMethodName_name.0, i32 0, i32 0); name
	}
], align 8; end of 'mm_method_names' array


attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable willreturn writeonly "frame-pointer"="non-leaf" "target-cpu"="generic" "target-features"="+neon,+outline-atomics" }
attributes #1 = { "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable "frame-pointer"="non-leaf" "target-cpu"="generic" "target-features"="+neon,+outline-atomics" }
attributes #2 = { nounwind }

!llvm.module.flags = !{!0, !1, !2, !3, !4, !5}
!llvm.ident = !{!6}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 1, !"branch-target-enforcement", i32 0}
!3 = !{i32 1, !"sign-return-address", i32 0}
!4 = !{i32 1, !"sign-return-address-all", i32 0}
!5 = !{i32 1, !"sign-return-address-with-bkey", i32 0}
!6 = !{!"Xamarin.Android remotes/origin/d17-5 @ a200af12c1e846626b8caebd926ac14c185f71ec"}
!llvm.linker.options = !{}
