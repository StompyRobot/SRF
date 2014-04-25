using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stateless
{
    static class ParameterConversion
    {
        public static object Unpack(object[] args, Type argType, int index)
        {
            Enforce.ArgumentNotNull(args, "args");

            if (args.Length <= index)
                throw new ArgumentException(
					string.Format("An argument of type {0} is required in position {1}.", argType, index));

            var arg = args[index];

            if (arg != null && !argType.IsAssignableFrom(arg.GetType()))
                throw new ArgumentException(
					string.Format("The argument in position {0} is of type {1} but must be of type {2}.", index, arg.GetType(), argType));

            return arg;
        }

        public static TArg Unpack<TArg>(object[] args, int index)
        {
            return (TArg)Unpack(args, typeof(TArg), index);
        }

        public static void Validate(object[] args, Type[] expected)
        {
            if (args.Length > expected.Length)
                throw new ArgumentException(
					string.Format("Too many parameters have been supplied. Expecting {0} but got {1}.", expected.Length, args.Length));

            for (int i = 0; i < expected.Length; ++i)
                Unpack(args, expected[i], i);
        }
    }
}
