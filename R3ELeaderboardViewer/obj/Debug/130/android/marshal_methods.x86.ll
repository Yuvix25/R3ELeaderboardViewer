; ModuleID = 'obj\Debug\130\android\marshal_methods.x86.ll'
source_filename = "obj\Debug\130\android\marshal_methods.x86.ll"
target datalayout = "e-m:e-p:32:32-p270:32:32-p271:32:32-p272:64:64-f64:32:64-f80:32-n8:16:32-S128"
target triple = "i686-unknown-linux-android"


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
@assembly_image_cache = local_unnamed_addr global [0 x %struct.MonoImage*] zeroinitializer, align 4
; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = local_unnamed_addr constant [298 x i32] [
	i32 6657927, ; 0: Xamarin.Grpc.Protobuf.Lite.dll => 0x659787 => 132
	i32 9414545, ; 1: Xamarin.Grpc.Android => 0x8fa791 => 127
	i32 32687329, ; 2: Xamarin.AndroidX.Lifecycle.Runtime => 0x1f2c4e1 => 77
	i32 34715100, ; 3: Xamarin.Google.Guava.ListenableFuture.dll => 0x211b5dc => 118
	i32 57305218, ; 4: Xamarin.KotlinX.Coroutines.Play.Services => 0x36a6882 => 144
	i32 57967248, ; 5: Xamarin.Android.Support.VersionedParcelable.dll => 0x3748290 => 40
	i32 101534019, ; 6: Xamarin.AndroidX.SlidingPaneLayout => 0x60d4943 => 88
	i32 103834273, ; 7: Xamarin.Firebase.Annotations.dll => 0x63062a1 => 102
	i32 120558881, ; 8: Xamarin.AndroidX.SlidingPaneLayout.dll => 0x72f9521 => 88
	i32 134690465, ; 9: Xamarin.Kotlin.StdLib.Jdk7.dll => 0x80736a1 => 139
	i32 160529393, ; 10: Xamarin.Android.Arch.Core.Common => 0x9917bf1 => 15
	i32 165246403, ; 11: Xamarin.AndroidX.Collection.dll => 0x9d975c3 => 53
	i32 166922606, ; 12: Xamarin.Android.Support.Compat.dll => 0x9f3096e => 25
	i32 182336117, ; 13: Xamarin.AndroidX.SwipeRefreshLayout.dll => 0xade3a75 => 90
	i32 201930040, ; 14: Xamarin.Android.Arch.Core.Runtime => 0xc093538 => 16
	i32 209399409, ; 15: Xamarin.AndroidX.Browser.dll => 0xc7b2e71 => 51
	i32 227054016, ; 16: Xamarin.GoogleAndroid.Annotations.dll => 0xd8891c0 => 119
	i32 230216969, ; 17: Xamarin.AndroidX.Legacy.Support.Core.Utils.dll => 0xdb8d509 => 72
	i32 261689757, ; 18: Xamarin.AndroidX.ConstraintLayout.dll => 0xf99119d => 57
	i32 266337479, ; 19: Xamarin.Google.Guava.FailureAccess.dll => 0xfdffcc7 => 117
	i32 271099684, ; 20: Xamarin.Grpc.OkHttp => 0x1028a724 => 131
	i32 278686392, ; 21: Xamarin.AndroidX.Lifecycle.LiveData.dll => 0x109c6ab8 => 75
	i32 280482487, ; 22: Xamarin.AndroidX.Interpolator => 0x10b7d2b7 => 70
	i32 293936332, ; 23: Xamarin.GooglePlayServices.Auth.Api.Phone.dll => 0x11851ccc => 120
	i32 318968648, ; 24: Xamarin.AndroidX.Activity.dll => 0x13031348 => 42
	i32 321597661, ; 25: System.Numerics => 0x132b30dd => 11
	i32 342366114, ; 26: Xamarin.AndroidX.Lifecycle.Common => 0x146817a2 => 73
	i32 389971796, ; 27: Xamarin.Android.Support.Core.UI => 0x173e7f54 => 27
	i32 441335492, ; 28: Xamarin.AndroidX.ConstraintLayout.Core => 0x1a4e3ec4 => 56
	i32 442521989, ; 29: Xamarin.Essentials => 0x1a605985 => 101
	i32 443493152, ; 30: Xamarin.Google.Android.Recaptcha => 0x1a6f2b20 => 114
	i32 450948140, ; 31: Xamarin.AndroidX.Fragment.dll => 0x1ae0ec2c => 69
	i32 453011810, ; 32: Xamarin.Firebase.Database.Collection.dll => 0x1b006962 => 109
	i32 465846621, ; 33: mscorlib => 0x1bc4415d => 5
	i32 469710990, ; 34: System.dll => 0x1bff388e => 9
	i32 476646585, ; 35: Xamarin.AndroidX.Interpolator.dll => 0x1c690cb9 => 70
	i32 486930444, ; 36: Xamarin.AndroidX.LocalBroadcastManager.dll => 0x1d05f80c => 81
	i32 493301629, ; 37: Xamarin.Firebase.AppCheck.Interop.dll => 0x1d672f7d => 103
	i32 514659665, ; 38: Xamarin.Android.Support.Compat => 0x1ead1551 => 25
	i32 524864063, ; 39: Xamarin.Android.Support.Print => 0x1f48ca3f => 37
	i32 527452488, ; 40: Xamarin.Kotlin.StdLib.Jdk7 => 0x1f704948 => 139
	i32 557405415, ; 41: Jsr305Binding => 0x213954e7 => 3
	i32 569601784, ; 42: Xamarin.AndroidX.Window.Extensions.Core.Core => 0x21f36ef8 => 99
	i32 589597883, ; 43: Xamarin.GooglePlayServices.Auth.Api.Phone => 0x23248cbb => 120
	i32 627609679, ; 44: Xamarin.AndroidX.CustomView => 0x2568904f => 62
	i32 639843206, ; 45: Xamarin.AndroidX.Emoji2.ViewsHelper.dll => 0x26233b86 => 68
	i32 663517072, ; 46: Xamarin.AndroidX.VersionedParcelable => 0x278c7790 => 95
	i32 666292255, ; 47: Xamarin.AndroidX.Arch.Core.Common.dll => 0x27b6d01f => 48
	i32 691348768, ; 48: Xamarin.KotlinX.Coroutines.Android.dll => 0x29352520 => 141
	i32 692692150, ; 49: Xamarin.Android.Support.Annotations => 0x2949a4b6 => 22
	i32 700284507, ; 50: Xamarin.Jetbrains.Annotations => 0x29bd7e5b => 136
	i32 712915335, ; 51: Xamarin.Grpc.Api => 0x2a7e3987 => 128
	i32 714456728, ; 52: Square.OkIO.JVM.dll => 0x2a95be98 => 7
	i32 720511267, ; 53: Xamarin.Kotlin.StdLib.Jdk8 => 0x2af22123 => 140
	i32 762010972, ; 54: R3ELeaderboardViewer => 0x2d6b5d5c => 0
	i32 763781610, ; 55: Xamarin.Google.Android.Play.Integrity => 0x2d8661ea => 113
	i32 790371945, ; 56: Xamarin.AndroidX.CustomView.PoolingContainer.dll => 0x2f1c1e69 => 63
	i32 801787702, ; 57: Xamarin.Android.Support.Interpolator => 0x2fca4f36 => 34
	i32 809851609, ; 58: System.Drawing.Common.dll => 0x30455ad9 => 146
	i32 843511501, ; 59: Xamarin.AndroidX.Print => 0x3246f6cd => 83
	i32 916714535, ; 60: Xamarin.Android.Support.Print.dll => 0x36a3f427 => 37
	i32 928116545, ; 61: Xamarin.Google.Guava.ListenableFuture => 0x3751ef41 => 118
	i32 956575887, ; 62: Xamarin.Kotlin.StdLib.Jdk8.dll => 0x3904308f => 140
	i32 961995525, ; 63: Square.OkIO.dll => 0x3956e305 => 6
	i32 967690846, ; 64: Xamarin.AndroidX.Lifecycle.Common.dll => 0x39adca5e => 73
	i32 987342438, ; 65: Xamarin.Android.Arch.Lifecycle.LiveData.dll => 0x3ad9a666 => 19
	i32 1012816738, ; 66: Xamarin.AndroidX.SavedState.dll => 0x3c5e5b62 => 87
	i32 1031528504, ; 67: Xamarin.Google.ErrorProne.Annotations.dll => 0x3d7be038 => 115
	i32 1035644815, ; 68: Xamarin.AndroidX.AppCompat => 0x3dbaaf8f => 47
	i32 1052210849, ; 69: Xamarin.AndroidX.Lifecycle.ViewModel.dll => 0x3eb776a1 => 78
	i32 1067306892, ; 70: GoogleGson => 0x3f9dcf8c => 1
	i32 1084122840, ; 71: Xamarin.Kotlin.StdLib => 0x409e66d8 => 138
	i32 1098167829, ; 72: Xamarin.Android.Arch.Lifecycle.ViewModel => 0x4174b615 => 21
	i32 1098259244, ; 73: System => 0x41761b2c => 9
	i32 1110581358, ; 74: Xamarin.Firebase.Auth => 0x4232206e => 104
	i32 1111591002, ; 75: Xamarin.CodeHaus.Mojo.AnimalSnifferAnnotations => 0x4241885a => 100
	i32 1149092582, ; 76: Xamarin.AndroidX.Window => 0x447dc2e6 => 98
	i32 1159499262, ; 77: Xamarin.Grpc.Stub.dll => 0x451c8dfe => 133
	i32 1175144683, ; 78: Xamarin.AndroidX.VectorDrawable.Animated => 0x460b48eb => 93
	i32 1204270330, ; 79: Xamarin.AndroidX.Arch.Core.Common => 0x47c7b4fa => 48
	i32 1230765884, ; 80: Xamarin.Grpc.Stub => 0x495bff3c => 133
	i32 1243150071, ; 81: Xamarin.AndroidX.Window.Extensions.Core.Core.dll => 0x4a18f6f7 => 99
	i32 1246548578, ; 82: Xamarin.AndroidX.Collection.Jvm.dll => 0x4a4cd262 => 54
	i32 1263886435, ; 83: Xamarin.Google.Guava.dll => 0x4b556063 => 116
	i32 1264511973, ; 84: Xamarin.AndroidX.Startup.StartupRuntime.dll => 0x4b5eebe5 => 89
	i32 1264890200, ; 85: Xamarin.KotlinX.Coroutines.Core.dll => 0x4b64b158 => 142
	i32 1267360935, ; 86: Xamarin.AndroidX.VectorDrawable => 0x4b8a64a7 => 94
	i32 1275534314, ; 87: Xamarin.KotlinX.Coroutines.Android => 0x4c071bea => 141
	i32 1278448581, ; 88: Xamarin.AndroidX.Annotation.Jvm => 0x4c3393c5 => 45
	i32 1292763917, ; 89: Xamarin.Android.Support.CursorAdapter.dll => 0x4d0e030d => 29
	i32 1293217323, ; 90: Xamarin.AndroidX.DrawerLayout.dll => 0x4d14ee2b => 65
	i32 1297413738, ; 91: Xamarin.Android.Arch.Lifecycle.LiveData.Core => 0x4d54f66a => 18
	i32 1316849161, ; 92: Xamarin.Io.PerfMark.PerfMarkApi => 0x4e7d8609 => 134
	i32 1322716291, ; 93: Xamarin.AndroidX.Window.dll => 0x4ed70c83 => 98
	i32 1333047053, ; 94: Xamarin.Firebase.Common => 0x4f74af0d => 106
	i32 1376866003, ; 95: Xamarin.AndroidX.SavedState => 0x52114ed3 => 87
	i32 1379897097, ; 96: Xamarin.JavaX.Inject => 0x523f8f09 => 135
	i32 1406073936, ; 97: Xamarin.AndroidX.CoordinatorLayout => 0x53cefc50 => 58
	i32 1406299041, ; 98: Xamarin.Google.Guava.FailureAccess => 0x53d26ba1 => 117
	i32 1411702249, ; 99: Xamarin.Firebase.Auth.Interop.dll => 0x5424dde9 => 105
	i32 1445445088, ; 100: Xamarin.Android.Support.Fragment => 0x5627bde0 => 33
	i32 1469204771, ; 101: Xamarin.AndroidX.AppCompat.AppCompatResources => 0x57924923 => 46
	i32 1544135863, ; 102: Xamarin.Grpc.Api.dll => 0x5c09a4b7 => 128
	i32 1574652163, ; 103: Xamarin.Android.Support.Core.Utils.dll => 0x5ddb4903 => 28
	i32 1582372066, ; 104: Xamarin.AndroidX.DocumentFile.dll => 0x5e5114e2 => 64
	i32 1587447679, ; 105: Xamarin.Android.Arch.Core.Common.dll => 0x5e9e877f => 15
	i32 1597949149, ; 106: Xamarin.Google.ErrorProne.Annotations => 0x5f3ec4dd => 115
	i32 1622152042, ; 107: Xamarin.AndroidX.Loader.dll => 0x60b0136a => 80
	i32 1624863272, ; 108: Xamarin.AndroidX.ViewPager2 => 0x60d97228 => 97
	i32 1635184631, ; 109: Xamarin.AndroidX.Emoji2.ViewsHelper => 0x6176eff7 => 68
	i32 1636009525, ; 110: Xamarin.GooglePlayServices.Fido => 0x61838635 => 125
	i32 1636350590, ; 111: Xamarin.AndroidX.CursorAdapter => 0x6188ba7e => 61
	i32 1639515021, ; 112: System.Net.Http.dll => 0x61b9038d => 10
	i32 1657153582, ; 113: System.Runtime => 0x62c6282e => 13
	i32 1658241508, ; 114: Xamarin.AndroidX.Tracing.Tracing.dll => 0x62d6c1e4 => 91
	i32 1658251792, ; 115: Xamarin.Google.Android.Material.dll => 0x62d6ea10 => 112
	i32 1664238415, ; 116: Xamarin.Firebase.Database.Collection => 0x6332434f => 109
	i32 1670060433, ; 117: Xamarin.AndroidX.ConstraintLayout => 0x638b1991 => 57
	i32 1698840827, ; 118: Xamarin.Kotlin.StdLib.Common => 0x654240fb => 137
	i32 1729485958, ; 119: Xamarin.AndroidX.CardView.dll => 0x6715dc86 => 52
	i32 1748729059, ; 120: Xamarin.GoogleAndroid.Annotations => 0x683b7ce3 => 119
	i32 1766324549, ; 121: Xamarin.AndroidX.SwipeRefreshLayout => 0x6947f945 => 90
	i32 1776026572, ; 122: System.Core.dll => 0x69dc03cc => 8
	i32 1788241197, ; 123: Xamarin.AndroidX.Fragment => 0x6a96652d => 69
	i32 1808609942, ; 124: Xamarin.AndroidX.Loader => 0x6bcd3296 => 80
	i32 1813058853, ; 125: Xamarin.Kotlin.StdLib.dll => 0x6c111525 => 138
	i32 1813201214, ; 126: Xamarin.Google.Android.Material => 0x6c13413e => 112
	i32 1866717392, ; 127: Xamarin.Android.Support.Interpolator.dll => 0x6f43d8d0 => 34
	i32 1867746548, ; 128: Xamarin.Essentials.dll => 0x6f538cf4 => 101
	i32 1875053220, ; 129: Xamarin.Firebase.Auth.Interop => 0x6fc30aa4 => 105
	i32 1885316902, ; 130: Xamarin.AndroidX.Arch.Core.Runtime.dll => 0x705fa726 => 49
	i32 1908813208, ; 131: Xamarin.GooglePlayServices.Basement => 0x71c62d98 => 124
	i32 1916660109, ; 132: Xamarin.Android.Arch.Lifecycle.ViewModel.dll => 0x723de98d => 21
	i32 1919157823, ; 133: Xamarin.AndroidX.MultiDex.dll => 0x7264063f => 82
	i32 1983156543, ; 134: Xamarin.Kotlin.StdLib.Common.dll => 0x7634913f => 137
	i32 2019465201, ; 135: Xamarin.AndroidX.Lifecycle.ViewModel => 0x785e97f1 => 78
	i32 2037417872, ; 136: Xamarin.Android.Support.ViewPager => 0x79708790 => 41
	i32 2044222327, ; 137: Xamarin.Android.Support.Loader => 0x79d85b77 => 35
	i32 2055257422, ; 138: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 0x7a80bd4e => 74
	i32 2079903147, ; 139: System.Runtime.dll => 0x7bf8cdab => 13
	i32 2083657273, ; 140: Xamarin.Firebase.ProtoliteWellKnownTypes => 0x7c321639 => 111
	i32 2086218969, ; 141: Xamarin.Google.Android.Play.Integrity.dll => 0x7c592cd9 => 113
	i32 2090596640, ; 142: System.Numerics.Vectors => 0x7c9bf920 => 12
	i32 2095474518, ; 143: Xamarin.GooglePlayServices.Auth.Base => 0x7ce66756 => 121
	i32 2097448633, ; 144: Xamarin.AndroidX.Legacy.Support.Core.UI => 0x7d0486b9 => 71
	i32 2129483829, ; 145: Xamarin.GooglePlayServices.Base.dll => 0x7eed5835 => 123
	i32 2139458754, ; 146: Xamarin.Android.Support.DrawerLayout => 0x7f858cc2 => 32
	i32 2166116741, ; 147: Xamarin.Android.Support.Core.Utils => 0x811c5185 => 28
	i32 2174878672, ; 148: Xamarin.Firebase.Annotations => 0x81a203d0 => 102
	i32 2195564014, ; 149: Xamarin.Grpc.Context => 0x82dda5ee => 129
	i32 2196165013, ; 150: Xamarin.Android.Support.VersionedParcelable => 0x82e6d195 => 40
	i32 2201107256, ; 151: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 0x83323b38 => 143
	i32 2201231467, ; 152: System.Net.Http => 0x8334206b => 10
	i32 2217644978, ; 153: Xamarin.AndroidX.VectorDrawable.Animated.dll => 0x842e93b2 => 93
	i32 2225853105, ; 154: Xamarin.Firebase.Common.Ktx => 0x84abd2b1 => 107
	i32 2244775296, ; 155: Xamarin.AndroidX.LocalBroadcastManager => 0x85cc8d80 => 81
	i32 2256548716, ; 156: Xamarin.AndroidX.MultiDex => 0x8680336c => 82
	i32 2279755925, ; 157: Xamarin.AndroidX.RecyclerView.dll => 0x87e25095 => 85
	i32 2315684594, ; 158: Xamarin.AndroidX.Annotation.dll => 0x8a068af2 => 43
	i32 2330457430, ; 159: Xamarin.Android.Support.Core.UI.dll => 0x8ae7f556 => 27
	i32 2330986192, ; 160: Xamarin.Android.Support.SlidingPaneLayout.dll => 0x8af006d0 => 38
	i32 2373288475, ; 161: Xamarin.Android.Support.Fragment.dll => 0x8d75821b => 33
	i32 2382033717, ; 162: Xamarin.Firebase.Auth.dll => 0x8dfaf335 => 104
	i32 2403452196, ; 163: Xamarin.AndroidX.Emoji2.dll => 0x8f41c524 => 67
	i32 2440966767, ; 164: Xamarin.Android.Support.Loader.dll => 0x917e326f => 35
	i32 2465532216, ; 165: Xamarin.AndroidX.ConstraintLayout.Core.dll => 0x92f50938 => 56
	i32 2475788418, ; 166: Java.Interop.dll => 0x93918882 => 2
	i32 2487632829, ; 167: Xamarin.Android.Support.DocumentFile => 0x944643bd => 31
	i32 2505896520, ; 168: Xamarin.AndroidX.Lifecycle.Runtime.dll => 0x955cf248 => 77
	i32 2561374756, ; 169: Xamarin.Google.Android.Recaptcha.dll => 0x98ab7a24 => 114
	i32 2581819634, ; 170: Xamarin.AndroidX.VectorDrawable.dll => 0x99e370f2 => 94
	i32 2591433303, ; 171: Xamarin.Grpc.Core.dll => 0x9a762257 => 130
	i32 2605712449, ; 172: Xamarin.KotlinX.Coroutines.Core.Jvm => 0x9b500441 => 143
	i32 2620871830, ; 173: Xamarin.AndroidX.CursorAdapter.dll => 0x9c375496 => 61
	i32 2624644809, ; 174: Xamarin.AndroidX.DynamicAnimation => 0x9c70e6c9 => 66
	i32 2633051222, ; 175: Xamarin.AndroidX.Lifecycle.LiveData => 0x9cf12c56 => 75
	i32 2640452924, ; 176: Xamarin.Grpc.Protobuf.Lite => 0x9d621d3c => 132
	i32 2652665316, ; 177: Xamarin.CodeHaus.Mojo.AnimalSnifferAnnotations.dll => 0x9e1c75e4 => 100
	i32 2671474046, ; 178: Xamarin.KotlinX.Coroutines.Core => 0x9f3b757e => 142
	i32 2698266930, ; 179: Xamarin.Android.Arch.Lifecycle.LiveData => 0xa0d44932 => 19
	i32 2701096212, ; 180: Xamarin.AndroidX.Tracing.Tracing => 0xa0ff7514 => 91
	i32 2715831284, ; 181: Xamarin.Firebase.ProtoliteWellKnownTypes.dll => 0xa1e04bf4 => 111
	i32 2732626843, ; 182: Xamarin.AndroidX.Activity => 0xa2e0939b => 42
	i32 2737747696, ; 183: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 0xa32eb6f0 => 46
	i32 2751899777, ; 184: Xamarin.Android.Support.Collections => 0xa406a881 => 24
	i32 2752363754, ; 185: Xamarin.Firebase.Firestore.dll => 0xa40dbcea => 110
	i32 2765528135, ; 186: Xamarin.Io.PerfMark.PerfMarkApi.dll => 0xa4d69c47 => 134
	i32 2770495804, ; 187: Xamarin.Jetbrains.Annotations.dll => 0xa522693c => 136
	i32 2778768386, ; 188: Xamarin.AndroidX.ViewPager.dll => 0xa5a0a402 => 96
	i32 2779977773, ; 189: Xamarin.AndroidX.ResourceInspection.Annotation.dll => 0xa5b3182d => 86
	i32 2788639665, ; 190: Xamarin.Android.Support.LocalBroadcastManager => 0xa63743b1 => 36
	i32 2788775637, ; 191: Xamarin.Android.Support.SwipeRefreshLayout.dll => 0xa63956d5 => 39
	i32 2799649579, ; 192: Xamarin.Protobuf.JavaLite.dll => 0xa6df432b => 145
	i32 2804607052, ; 193: Xamarin.Firebase.Components.dll => 0xa72ae84c => 108
	i32 2810250172, ; 194: Xamarin.AndroidX.CoordinatorLayout.dll => 0xa78103bc => 58
	i32 2819470561, ; 195: System.Xml.dll => 0xa80db4e1 => 14
	i32 2821294376, ; 196: Xamarin.AndroidX.ResourceInspection.Annotation => 0xa8298928 => 86
	i32 2847418871, ; 197: Xamarin.GooglePlayServices.Base => 0xa9b829f7 => 123
	i32 2853208004, ; 198: Xamarin.AndroidX.ViewPager => 0xaa107fc4 => 96
	i32 2855708567, ; 199: Xamarin.AndroidX.Transition => 0xaa36a797 => 92
	i32 2856624150, ; 200: Xamarin.Grpc.Core => 0xaa44a016 => 130
	i32 2870458124, ; 201: Xamarin.Firebase.AppCheck.Interop => 0xab17b70c => 103
	i32 2875164099, ; 202: Jsr305Binding.dll => 0xab5f85c3 => 3
	i32 2876493027, ; 203: Xamarin.Android.Support.SwipeRefreshLayout => 0xab73cce3 => 39
	i32 2893257763, ; 204: Xamarin.Android.Arch.Core.Runtime.dll => 0xac739c23 => 16
	i32 2903344695, ; 205: System.ComponentModel.Composition => 0xad0d8637 => 148
	i32 2905242038, ; 206: mscorlib.dll => 0xad2a79b6 => 5
	i32 2916838712, ; 207: Xamarin.AndroidX.ViewPager2.dll => 0xaddb6d38 => 97
	i32 2919462931, ; 208: System.Numerics.Vectors.dll => 0xae037813 => 12
	i32 2921128767, ; 209: Xamarin.AndroidX.Annotation.Experimental.dll => 0xae1ce33f => 44
	i32 2921692953, ; 210: Xamarin.Android.Support.CustomView.dll => 0xae257f19 => 30
	i32 2943219317, ; 211: Square.OkIO => 0xaf6df675 => 6
	i32 2960379616, ; 212: Xamarin.Google.Guava => 0xb073cee0 => 116
	i32 2977575538, ; 213: R3ELeaderboardViewer.dll => 0xb17a3272 => 0
	i32 2978675010, ; 214: Xamarin.AndroidX.DrawerLayout => 0xb18af942 => 65
	i32 2996846495, ; 215: Xamarin.AndroidX.Lifecycle.Process.dll => 0xb2a03f9f => 76
	i32 3016983068, ; 216: Xamarin.AndroidX.Startup.StartupRuntime => 0xb3d3821c => 89
	i32 3024354802, ; 217: Xamarin.AndroidX.Legacy.Support.Core.Utils => 0xb443fdf2 => 72
	i32 3056250942, ; 218: Xamarin.Android.Support.AsyncLayoutInflater.dll => 0xb62ab03e => 23
	i32 3058099980, ; 219: Xamarin.GooglePlayServices.Tasks => 0xb646e70c => 126
	i32 3068715062, ; 220: Xamarin.Android.Arch.Lifecycle.Common => 0xb6e8e036 => 17
	i32 3071899978, ; 221: Xamarin.Firebase.Common.dll => 0xb719794a => 106
	i32 3148237826, ; 222: GoogleGson.dll => 0xbba64c02 => 1
	i32 3150271759, ; 223: Xamarin.KotlinX.Coroutines.Play.Services.dll => 0xbbc5550f => 144
	i32 3204912593, ; 224: Xamarin.Android.Support.AsyncLayoutInflater => 0xbf0715d1 => 23
	i32 3211777861, ; 225: Xamarin.AndroidX.DocumentFile => 0xbf6fd745 => 64
	i32 3222740722, ; 226: Xamarin.Protobuf.JavaLite => 0xc0171ef2 => 145
	i32 3230271625, ; 227: Square.OkIO.JVM => 0xc08a0889 => 7
	i32 3230466174, ; 228: Xamarin.GooglePlayServices.Basement.dll => 0xc08d007e => 124
	i32 3233339011, ; 229: Xamarin.Android.Arch.Lifecycle.LiveData.Core.dll => 0xc0b8d683 => 18
	i32 3247949154, ; 230: Mono.Security => 0xc197c562 => 147
	i32 3258312781, ; 231: Xamarin.AndroidX.CardView => 0xc235e84d => 52
	i32 3267021929, ; 232: Xamarin.AndroidX.AsyncLayoutInflater => 0xc2bacc69 => 50
	i32 3296380511, ; 233: Xamarin.Android.Support.Collections.dll => 0xc47ac65f => 24
	i32 3317135071, ; 234: Xamarin.AndroidX.CustomView.dll => 0xc5b776df => 62
	i32 3321585405, ; 235: Xamarin.Android.Support.DocumentFile.dll => 0xc5fb5efd => 31
	i32 3340431453, ; 236: Xamarin.AndroidX.Arch.Core.Runtime => 0xc71af05d => 49
	i32 3345895724, ; 237: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller.dll => 0xc76e512c => 84
	i32 3352662376, ; 238: Xamarin.Android.Support.CoordinaterLayout => 0xc7d59168 => 26
	i32 3353484488, ; 239: Xamarin.AndroidX.Legacy.Support.Core.UI.dll => 0xc7e21cc8 => 71
	i32 3357663996, ; 240: Xamarin.Android.Support.CursorAdapter => 0xc821e2fc => 29
	i32 3362522851, ; 241: Xamarin.AndroidX.Core => 0xc86c06e3 => 60
	i32 3366347497, ; 242: Java.Interop => 0xc8a662e9 => 2
	i32 3374999561, ; 243: Xamarin.AndroidX.RecyclerView => 0xc92a6809 => 85
	i32 3396853433, ; 244: Xamarin.GooglePlayServices.Fido.dll => 0xca77deb9 => 125
	i32 3405233483, ; 245: Xamarin.AndroidX.CustomView.PoolingContainer => 0xcaf7bd4b => 63
	i32 3429136800, ; 246: System.Xml => 0xcc6479a0 => 14
	i32 3439690031, ; 247: Xamarin.Android.Support.Annotations.dll => 0xcd05812f => 22
	i32 3441283291, ; 248: Xamarin.AndroidX.DynamicAnimation.dll => 0xcd1dd0db => 66
	i32 3465947803, ; 249: Xamarin.GooglePlayServices.Auth.dll => 0xce962a9b => 122
	i32 3473879593, ; 250: Xamarin.Grpc.OkHttp.dll => 0xcf0f3229 => 131
	i32 3476120550, ; 251: Mono.Android => 0xcf3163e6 => 4
	i32 3493954962, ; 252: Xamarin.AndroidX.Concurrent.Futures.dll => 0xd0418592 => 55
	i32 3501239056, ; 253: Xamarin.AndroidX.AsyncLayoutInflater.dll => 0xd0b0ab10 => 50
	i32 3547625832, ; 254: Xamarin.Android.Support.SlidingPaneLayout => 0xd3747968 => 38
	i32 3567349600, ; 255: System.ComponentModel.Composition.dll => 0xd4a16f60 => 148
	i32 3597794883, ; 256: Xamarin.Firebase.Firestore => 0xd671fe43 => 110
	i32 3627220390, ; 257: Xamarin.AndroidX.Print.dll => 0xd832fda6 => 83
	i32 3633644679, ; 258: Xamarin.AndroidX.Annotation.Experimental => 0xd8950487 => 44
	i32 3641597786, ; 259: Xamarin.AndroidX.Lifecycle.LiveData.Core => 0xd90e5f5a => 74
	i32 3664423555, ; 260: Xamarin.Android.Support.ViewPager.dll => 0xda6aaa83 => 41
	i32 3672681054, ; 261: Mono.Android.dll => 0xdae8aa5e => 4
	i32 3681174138, ; 262: Xamarin.Android.Arch.Lifecycle.Common.dll => 0xdb6a427a => 17
	i32 3682565725, ; 263: Xamarin.AndroidX.Browser => 0xdb7f7e5d => 51
	i32 3684561358, ; 264: Xamarin.AndroidX.Concurrent.Futures => 0xdb9df1ce => 55
	i32 3689375977, ; 265: System.Drawing.Common => 0xdbe768e9 => 146
	i32 3706696989, ; 266: Xamarin.AndroidX.Core.Core.Ktx.dll => 0xdcefb51d => 59
	i32 3714038699, ; 267: Xamarin.Android.Support.LocalBroadcastManager.dll => 0xdd5fbbab => 36
	i32 3718780102, ; 268: Xamarin.AndroidX.Annotation => 0xdda814c6 => 43
	i32 3729924096, ; 269: Xamarin.GooglePlayServices.Auth => 0xde522000 => 122
	i32 3776062606, ; 270: Xamarin.Android.Support.DrawerLayout.dll => 0xe112248e => 32
	i32 3786282454, ; 271: Xamarin.AndroidX.Collection => 0xe1ae15d6 => 53
	i32 3829621856, ; 272: System.Numerics.dll => 0xe4436460 => 11
	i32 3832731800, ; 273: Xamarin.Android.Support.CoordinaterLayout.dll => 0xe472d898 => 26
	i32 3862817207, ; 274: Xamarin.Android.Arch.Lifecycle.Runtime.dll => 0xe63de9b7 => 20
	i32 3874897629, ; 275: Xamarin.Android.Arch.Lifecycle.Runtime => 0xe6f63edd => 20
	i32 3885922214, ; 276: Xamarin.AndroidX.Transition.dll => 0xe79e77a6 => 92
	i32 3888767677, ; 277: Xamarin.AndroidX.ProfileInstaller.ProfileInstaller => 0xe7c9e2bd => 84
	i32 3896760992, ; 278: Xamarin.AndroidX.Core.dll => 0xe843daa0 => 60
	i32 3910130544, ; 279: Xamarin.AndroidX.Collection.Jvm => 0xe90fdb70 => 54
	i32 3921031405, ; 280: Xamarin.AndroidX.VersionedParcelable.dll => 0xe9b630ed => 95
	i32 3934056515, ; 281: Xamarin.JavaX.Inject.dll => 0xea7cf043 => 135
	i32 3943739589, ; 282: Xamarin.Grpc.Context.dll => 0xeb10b0c5 => 129
	i32 3955647286, ; 283: Xamarin.AndroidX.AppCompat.dll => 0xebc66336 => 47
	i32 3959773229, ; 284: Xamarin.AndroidX.Lifecycle.Process => 0xec05582d => 76
	i32 3970018735, ; 285: Xamarin.GooglePlayServices.Tasks.dll => 0xeca1adaf => 126
	i32 3991193666, ; 286: Xamarin.GooglePlayServices.Auth.Base.dll => 0xede4c842 => 121
	i32 4015948917, ; 287: Xamarin.AndroidX.Annotation.Jvm.dll => 0xef5e8475 => 45
	i32 4063435586, ; 288: Xamarin.Android.Support.CustomView => 0xf2331b42 => 30
	i32 4101593132, ; 289: Xamarin.AndroidX.Emoji2 => 0xf479582c => 67
	i32 4105002889, ; 290: Mono.Security.dll => 0xf4ad5f89 => 147
	i32 4151237749, ; 291: System.Core => 0xf76edc75 => 8
	i32 4157403133, ; 292: Xamarin.Firebase.Common.Ktx.dll => 0xf7cceffd => 107
	i32 4182413190, ; 293: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 0xf94a8f86 => 79
	i32 4223148364, ; 294: Xamarin.Grpc.Android.dll => 0xfbb8214c => 127
	i32 4256097574, ; 295: Xamarin.AndroidX.Core.Core.Ktx => 0xfdaee526 => 59
	i32 4284549794, ; 296: Xamarin.Firebase.Components => 0xff610aa2 => 108
	i32 4292120959 ; 297: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 0xffd4917f => 79
], align 4
@assembly_image_cache_indices = local_unnamed_addr constant [298 x i32] [
	i32 132, i32 127, i32 77, i32 118, i32 144, i32 40, i32 88, i32 102, ; 0..7
	i32 88, i32 139, i32 15, i32 53, i32 25, i32 90, i32 16, i32 51, ; 8..15
	i32 119, i32 72, i32 57, i32 117, i32 131, i32 75, i32 70, i32 120, ; 16..23
	i32 42, i32 11, i32 73, i32 27, i32 56, i32 101, i32 114, i32 69, ; 24..31
	i32 109, i32 5, i32 9, i32 70, i32 81, i32 103, i32 25, i32 37, ; 32..39
	i32 139, i32 3, i32 99, i32 120, i32 62, i32 68, i32 95, i32 48, ; 40..47
	i32 141, i32 22, i32 136, i32 128, i32 7, i32 140, i32 0, i32 113, ; 48..55
	i32 63, i32 34, i32 146, i32 83, i32 37, i32 118, i32 140, i32 6, ; 56..63
	i32 73, i32 19, i32 87, i32 115, i32 47, i32 78, i32 1, i32 138, ; 64..71
	i32 21, i32 9, i32 104, i32 100, i32 98, i32 133, i32 93, i32 48, ; 72..79
	i32 133, i32 99, i32 54, i32 116, i32 89, i32 142, i32 94, i32 141, ; 80..87
	i32 45, i32 29, i32 65, i32 18, i32 134, i32 98, i32 106, i32 87, ; 88..95
	i32 135, i32 58, i32 117, i32 105, i32 33, i32 46, i32 128, i32 28, ; 96..103
	i32 64, i32 15, i32 115, i32 80, i32 97, i32 68, i32 125, i32 61, ; 104..111
	i32 10, i32 13, i32 91, i32 112, i32 109, i32 57, i32 137, i32 52, ; 112..119
	i32 119, i32 90, i32 8, i32 69, i32 80, i32 138, i32 112, i32 34, ; 120..127
	i32 101, i32 105, i32 49, i32 124, i32 21, i32 82, i32 137, i32 78, ; 128..135
	i32 41, i32 35, i32 74, i32 13, i32 111, i32 113, i32 12, i32 121, ; 136..143
	i32 71, i32 123, i32 32, i32 28, i32 102, i32 129, i32 40, i32 143, ; 144..151
	i32 10, i32 93, i32 107, i32 81, i32 82, i32 85, i32 43, i32 27, ; 152..159
	i32 38, i32 33, i32 104, i32 67, i32 35, i32 56, i32 2, i32 31, ; 160..167
	i32 77, i32 114, i32 94, i32 130, i32 143, i32 61, i32 66, i32 75, ; 168..175
	i32 132, i32 100, i32 142, i32 19, i32 91, i32 111, i32 42, i32 46, ; 176..183
	i32 24, i32 110, i32 134, i32 136, i32 96, i32 86, i32 36, i32 39, ; 184..191
	i32 145, i32 108, i32 58, i32 14, i32 86, i32 123, i32 96, i32 92, ; 192..199
	i32 130, i32 103, i32 3, i32 39, i32 16, i32 148, i32 5, i32 97, ; 200..207
	i32 12, i32 44, i32 30, i32 6, i32 116, i32 0, i32 65, i32 76, ; 208..215
	i32 89, i32 72, i32 23, i32 126, i32 17, i32 106, i32 1, i32 144, ; 216..223
	i32 23, i32 64, i32 145, i32 7, i32 124, i32 18, i32 147, i32 52, ; 224..231
	i32 50, i32 24, i32 62, i32 31, i32 49, i32 84, i32 26, i32 71, ; 232..239
	i32 29, i32 60, i32 2, i32 85, i32 125, i32 63, i32 14, i32 22, ; 240..247
	i32 66, i32 122, i32 131, i32 4, i32 55, i32 50, i32 38, i32 148, ; 248..255
	i32 110, i32 83, i32 44, i32 74, i32 41, i32 4, i32 17, i32 51, ; 256..263
	i32 55, i32 146, i32 59, i32 36, i32 43, i32 122, i32 32, i32 53, ; 264..271
	i32 11, i32 26, i32 20, i32 20, i32 92, i32 84, i32 60, i32 54, ; 272..279
	i32 95, i32 135, i32 129, i32 47, i32 76, i32 126, i32 121, i32 45, ; 280..287
	i32 30, i32 67, i32 147, i32 8, i32 107, i32 79, i32 127, i32 59, ; 288..295
	i32 108, i32 79 ; 296..297
], align 4

@marshal_methods_number_of_classes = local_unnamed_addr constant i32 0, align 4

; marshal_methods_class_cache
@marshal_methods_class_cache = global [0 x %struct.MarshalMethodsManagedClass] [
], align 4; end of 'marshal_methods_class_cache' array


@get_function_pointer = internal unnamed_addr global void (i32, i32, i32, i8**)* null, align 4

; Function attributes: "frame-pointer"="none" "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" "stackrealign" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" uwtable willreturn writeonly
define void @xamarin_app_init (void (i32, i32, i32, i8**)* %fn) local_unnamed_addr #0
{
	store void (i32, i32, i32, i8**)* %fn, void (i32, i32, i32, i8**)** @get_function_pointer, align 4
	ret void
}

; Names of classes in which marshal methods reside
@mm_class_names = local_unnamed_addr constant [0 x i8*] zeroinitializer, align 4
@__MarshalMethodName_name.0 = internal constant [1 x i8] c"\00", align 1

; mm_method_names
@mm_method_names = local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	; 0
	%struct.MarshalMethodName {
		i64 0, ; id 0x0; name: 
		i8* getelementptr inbounds ([1 x i8], [1 x i8]* @__MarshalMethodName_name.0, i32 0, i32 0); name
	}
], align 8; end of 'mm_method_names' array


attributes #0 = { "min-legal-vector-width"="0" mustprogress nofree norecurse nosync "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable willreturn writeonly "frame-pointer"="none" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" "stackrealign" }
attributes #1 = { "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nounwind sspstrong "stack-protector-buffer-size"="8" uwtable "frame-pointer"="none" "target-cpu"="i686" "target-features"="+cx8,+mmx,+sse,+sse2,+sse3,+ssse3,+x87" "tune-cpu"="generic" "stackrealign" }
attributes #2 = { nounwind }

!llvm.module.flags = !{!0, !1, !2}
!llvm.ident = !{!3}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!2 = !{i32 1, !"NumRegisterParameters", i32 0}
!3 = !{!"Xamarin.Android remotes/origin/d17-5 @ a200af12c1e846626b8caebd926ac14c185f71ec"}
!llvm.linker.options = !{}
