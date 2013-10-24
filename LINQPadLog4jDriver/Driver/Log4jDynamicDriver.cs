using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using BerserkerDotNet.LINQPadLog4jDriver.Domain;
using LINQPad.Extensibility.DataContext;
using Microsoft.CSharp;

namespace BerserkerDotNet.LINQPadLog4jDriver
{
    //public class Log4jDynamicDriver : DynamicDataContextDriver
    //{
    //    public override string GetConnectionDescription(IConnectionInfo cxInfo)
    //    {
    //        return (string)cxInfo.DriverData.Element("Name") ?? "LogsView";
    //    }

    //    public override bool ShowConnectionDialog(IConnectionInfo cxInfo, bool isNewConnection)
    //    {
    //        var editor = new ConnectionStringEditor(cxInfo);

    //        return editor.ShowDialog() == true;
    //    }

    //    public override string Name
    //    {
    //        get { return "Log4j LINQPad Driver"; }
    //    }

    //    public override string Author
    //    {
    //        get { return "BerserkerDotNet"; }
    //    }

    //    public override List<ExplorerItem> GetSchemaAndBuildAssembly(IConnectionInfo cxInfo,
    //                                                                 AssemblyName assemblyToBuild, ref string nameSpace,
    //                                                                 ref string typeName)
    //    {
    //        nameSpace = "BerserkerDotNet.LINQPadLog4jDriver";
    //        typeName = "Log4jDataContext";

    //        AssemblyBuilder builder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyToBuild,
    //                                                                                AssemblyBuilderAccess.RunAndSave);
    //        ModuleBuilder modBuilder = builder.DefineDynamicModule("Driver");
    //        var typeBuilder = modBuilder.DefineType(nameSpace + "." + typeName, TypeAttributes.Public);
    //        var method = typeBuilder.DefineMethod("get_Logs", MethodAttributes.Public | MethodAttributes.SpecialName,
    //                                              typeof (IEnumerable<LogEntry>), new Type[0]);
    //        var gen = method.GetILGenerator();
    //        gen.DeclareLocal(typeof (LogEntry[]));
    //        gen.Emit(OpCodes.Ldc_I4_1);
    //        gen.Emit(OpCodes.Newarr, typeof (LogEntry));
    //        gen.Emit(OpCodes.Stloc_1);
    //        gen.Emit(OpCodes.Ldloc_1);
    //        gen.Emit(OpCodes.Ldc_I4_0);
    //        gen.Emit(OpCodes.Newobj, typeof (LogEntry).GetConstructor(new Type[0]));
    //        gen.Emit(OpCodes.Stelem_Ref);
    //        gen.Emit(OpCodes.Ldloc_1);
    //        gen.Emit(OpCodes.Stloc_0);
    //        gen.Emit(OpCodes.Ldloc_0);
    //        gen.Emit(OpCodes.Ret);
    //        var property = typeBuilder.DefineProperty("Logs", PropertyAttributes.None, typeof(IEnumerable<LogEntry>), new Type[0]);
    //        property.SetGetMethod(method);
    //        builder.Save(assemblyToBuild.Name + ".dll");

    //        var result = GetSchema();
    //        return result;
    //    }

    //    static void BuildAssembly(string code, AssemblyName name)
    //    {
    //        // Use the CSharpCodeProvider to compile the generated code:
    //        CompilerResults results;
    //        using (var codeProvider = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.5" } }))
    //        {
    //            var options = new CompilerParameters(
    //                "System.dll System.Core.dll System.Xml.dll".Split(),
    //                name.CodeBase,
    //                true);
    //            results = codeProvider.CompileAssemblyFromSource(options, code);
    //        }
    //        if (results.Errors.Count > 0)
    //            throw new Exception
    //                ("Cannot compile typed context: " + results.Errors[0].ErrorText + " (line " + results.Errors[0].Line + ")");
    //    }

    //    private static List<ExplorerItem> GetSchema()
    //    {
    //        var result = new List<ExplorerItem>();
    //        var root = new ExplorerItem("Logs", ExplorerItemKind.QueryableObject, ExplorerIcon.Table);
    //        var properties =
    //            typeof (LogEntry).GetProperties()
    //                             .Select(p => new ExplorerItem(p.Name, ExplorerItemKind.Property, ExplorerIcon.Column))
    //                             .ToList();
    //        root.Children = properties;
    //        result.Add(root);
    //        return result;
    //    }
    //}
}