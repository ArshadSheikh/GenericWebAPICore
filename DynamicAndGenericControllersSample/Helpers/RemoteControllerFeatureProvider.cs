using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using DynamicAndGenericControllersSample.Controllers;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;

namespace DynamicAndGenericControllersSample
{
    public class RemoteControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            //var remoteCode = new HttpClient().GetStringAsync("https://gist.githubusercontent.com/ArshadSheikh/70112ec670c75275979002a9d5126f79/raw/8be51fc6d6b64886eb99693e630d0d2659835e0c/ClassHirarchy.txt").GetAwaiter().GetResult();
            //if (remoteCode != null)
            //{
            //    var references = new List<MetadataReference>
            //    {
            //        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            //        MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
            //        MetadataReference.CreateFromFile(typeof(System.Runtime.AssemblyTargetedPatchBandAttribute).Assembly.Location),
            //        MetadataReference.CreateFromFile(typeof(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly.Location),
            //        MetadataReference.CreateFromFile(typeof(RemoteControllerFeatureProvider).Assembly.Location),
            //    };

            //    Assembly.GetEntryAssembly().GetReferencedAssemblies().ToList().ForEach(x=> references.Add(MetadataReference.CreateFromFile(Assembly.Load(x).Location)));

            //    var compilation = CSharpCompilation.Create("DynamicAssembly.dll", new[] { CSharpSyntaxTree.ParseText(remoteCode) },
            //        references,
            //        new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            //    using (var ms = new MemoryStream())
            //    {
            //        var emitResult = compilation.Emit(ms);

            //        if (!emitResult.Success)
            //        {
            //            // handle, log errors etc
            //            Debug.WriteLine("Compilation failed!");
            //            return;
            //        }

            //        ms.Seek(0, SeekOrigin.Begin);
            //        var assembly = Assembly.Load(ms.ToArray());
            //        var candidates = assembly.GetExportedTypes();

            //        foreach (var candidate in candidates)
            //        {
            //            feature.Controllers.Add(typeof(BaseController<>).MakeGenericType(candidate).GetTypeInfo());
            //        }
            //    }
            //}

            var candidates = Assembly.GetExecutingAssembly().GetTypes().Where(x=> x.Namespace == "DynamicAndGenericControllersSample.Models");
            foreach (var candidate in candidates)
                feature.Controllers.Add(typeof(BaseController<>).MakeGenericType(candidate).GetTypeInfo());

        }
    }
}
