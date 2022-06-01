﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Collections.Generic;
using MonoMod.Cil;

namespace TeaFramework.Features.Utility
{
    /// <summary>
    ///     IL utilities, mostly for logging.
    /// </summary>
    public static class ILHelper
    {
        /// <summary>
        ///     Prepares an instruction, extracting information used for logging.
        /// </summary>
        /// <param name="instr">The instruction to prepare.</param>
        /// <param name="offset">The IL offset, as a string.</param>
        /// <param name="opcode">The IL opcode, as a string.</param>
        /// <param name="operand">The IL operand, as a string.</param>
        public static void PrepareInstruction(Instruction instr, out string offset, out string opcode, out string operand) {
            offset = $"IL_{instr.Offset:X5}:";

            opcode = instr.OpCode.Name;

            switch (instr.Operand) {
                case null:
                    operand = "";
                    break;

                //This label's target should NEVER be null!  If it is, the IL edit wouldn't load anyway
                case ILLabel label:
                    operand = $"IL_{label.Target.Offset:X5}";
                    break;

                default:
                {
                    if (instr.OpCode == OpCodes.Switch)
                        operand = "(" + string.Join(", ", (instr.Operand as ILLabel[])!.Select(l => $"IL_{l.Target.Offset:X5}")) + ")";
                    else
                        operand = instr.Operand.ToString()!;
                    break;
                }
            }
        }

        /// <summary>
        ///     Logs information about an <see cref="ILCursor" /> object's method body. <br />
        /// </summary>
        /// <param name="c">The IL editing cursor</param>
        /// <param name="logFilePath">The destination file</param>
        public static void LogMethodBody(this ILCursor c, string logFilePath) {
            //Ensure that the instructions listed have the correct offset
            UpdateInstructionOffsets(c);

            int index = 0;

            Directory.CreateDirectory(new FileInfo(logFilePath).DirectoryName!);

            FileStream file = File.Open(logFilePath, FileMode.Create);

            using StreamWriter writer = new(file);

            writer.WriteLine(DateTime.Now.ToString("'['ddMMMyyyy '-' HH:mm:ss']'"));
            writer.WriteLine($"// ILCursor: {c.Method.Name}\n");

            writer.WriteLine("// Arguments:");

            if (c.Method.Parameters.Count == 0)
                writer.WriteLine($"{"none",8}");
            else
                foreach (ParameterDefinition? arg in c.Method.Parameters) {
                    string argIndex = $"[{arg.Index}]";
                    writer.WriteLine($"{argIndex,8} {arg.ParameterType.FullName} {arg.Name}");
                }

            writer.WriteLine();

            writer.WriteLine("// Locals:");

            if (!c.Body.HasVariables)
                writer.WriteLine($"{"none",8}");
            else
                foreach (VariableDefinition? local in c.Body.Variables) {
                    string localIndex = $"[{local.Index}]";
                    writer.WriteLine($"{localIndex,8} {local.VariableType.FullName} V_{local.Index}");
                }

            writer.WriteLine();

            writer.WriteLine("// Body:");
            do {
                PrepareInstruction(c.Instrs[index], out string offset, out string opcode, out string operand);

                writer.WriteLine($"{offset,-10}{opcode,-12} {operand}");
                index++;
            } while (index < c.Instrs.Count);
        }

        private static void UpdateInstructionOffsets(ILCursor c) {
            Collection<Instruction>? instrs = c.Instrs;
            int curOffset = 0;

            static Instruction[] ConvertToInstructions(ILLabel[] labels) {
                Instruction[] ret = new Instruction[labels.Length];

                for (int i = 0; i < labels.Length; i++) ret[i] = labels[i].Target;

                return ret;
            }

            foreach (Instruction? ins in instrs) {
                ins.Offset = curOffset;

                if (ins.OpCode != OpCodes.Switch) {
                    curOffset += ins.GetSize();
                }
                else {
                    //'switch' opcodes don't like having the operand as an ILLabel[] when calling GetSize()
                    //thus, this is required to even let the mod compile

                    Instruction copy = Instruction.Create(ins.OpCode, ConvertToInstructions((ILLabel[]) ins.Operand));
                    curOffset += copy.GetSize();
                }
            }
        }

        /// <summary>
        ///     Initializes automatic dumping of MonoMod assemblies to the tModLoader install directory. <br />
        ///     Currently does not work due to an issue in MonoMod.
        /// </summary>
        public static void InitMonoModDumps() {
            //Disable assembly dumping until this issue is fixed by MonoMod
            //see: https://discord.com/channels/103110554649894912/445276626352209920/953380019072270419
            bool noLog = false;
            if (noLog) return;

            Environment.SetEnvironmentVariable("MONOMOD_DMD_TYPE", "Auto");
            Environment.SetEnvironmentVariable("MONOMOD_DMD_DEBUG", "1");

            string dumpDir = Path.GetFullPath("MonoModDump");

            Directory.CreateDirectory(dumpDir);

            Environment.SetEnvironmentVariable("MONOMOD_DMD_DUMP", dumpDir);
        }

        /// <summary>
        ///     De-initializes automatic dumping of MonoMod assemblies to the tModLoader install directory. <br />
        ///     Currently does not work due to an issue in MonoMod.
        /// </summary>
        public static void DeInitMonoModDumps() {
            bool noLog = false;
            if (noLog) return;

            Environment.SetEnvironmentVariable("MONOMOD_DMD_DEBUG", "0");
        }

        /// <summary>
        ///     Gets the instruction at the given index, represented as a string.
        /// </summary>
        /// <param name="c">The IL cursor.</param>
        /// <param name="index">The instruction index.</param>
        /// <returns>The string-represented instruction.</returns>
        public static string GetInstructionString(ILCursor c, int index) {
            if (index < 0 || index >= c.Instrs.Count) return "ERROR: Index out of bounds.";

            PrepareInstruction(c.Instrs[index], out string offset, out string opcode, out string operand);

            return $"{offset} {opcode}   {operand}";
        }

        /// <summary>
        ///     Verifies that each <see cref="MemberInfo" /> is not null.
        /// </summary>
        /// <param name="memberInfos">
        ///     An array of <see cref="MemberInfo" /> objects, paired with an identifier used when throwing
        ///     the <see cref="NullReferenceException" /> if the object is null.
        /// </param>
        /// <exception cref="NullReferenceException"></exception>
        public static void EnsureAreNotNull(params (MemberInfo? member, string identifier)[] memberInfos) {
            foreach ((MemberInfo? member, string identifier) in memberInfos)
                if (member is null)
                    throw new NullReferenceException($"Member reference \"{identifier}\" is null");
        }
    }
}