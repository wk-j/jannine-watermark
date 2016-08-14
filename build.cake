#tool "nuget:?package=Fixie"
#addin "nuget:?package=Cake.Watch"
#addin "MagicChunks"

var solution = "Jannine.Watermark.sln";
var testProj = @"Cake.Highlight.Tests/Cake.Highlight.Tests.csproj";
var testDll = @"Cake.Highlight.tests/bin/Debug/Cake.Highlight.Tests.dll";

Action<string,string> build = (proj, target) => {
    MSBuild(proj, new MSBuildSettings {
        Verbosity = Verbosity.Minimal,
        ToolVersion = MSBuildToolVersion.VS2015,
        Configuration = "Debug",
        ToolPath = @"C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe",
        PlatformTarget = PlatformTarget.MSIL
    }.WithTarget(target));
};

Task("fixie")
    .Does(() => {
        build(testProj, "Build");
        var config = testDll + ".config";
        var className = Argument("className", "");
        TransformConfig(config, new TransformationCollection {
                { "configuration/appSettings/add[@key='fixie']/@value", className }
        });
        Fixie(testDll);
    });


Task("Reset-Experimental-Instance").Does(() => {
    var bin = @"C:\Program Files (x86)\Microsoft Visual Studio 14.0\VSSDK\VisualStudioIntegration\Tools\Bin\CreateExpInstance.exe";
    var args = "/Reset /VSInstance=14.0 /RootSuffix=Exp";
    StartProcess(bin, new ProcessSettings {
        Arguments = args
    });
});

Task("View-Log")
    .Does(() => {
        var path = @"C:\Users\wk\AppData\Roaming\Microsoft\VisualStudio\14.0Exp\ActivityLog.xml";
        StartProcess("notepad", new ProcessSettings { Arguments = path } );
    });

Task("Build-Debug")
    .Does(() => {
        build(solution, "Build");
    });

Task("Rebuild-Debug")
    .Does(() => {
        build(solution, "Rebuild");
    });

var target = Argument("target", "default");
RunTarget(target);