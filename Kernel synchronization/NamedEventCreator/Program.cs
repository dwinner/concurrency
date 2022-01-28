/**
 * Ручное получение именованного события
 */

using Microsoft.Win32.SafeHandles;
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace _16_NamedEventCreator
{
   class Program
   {
      static void Main()
      {
      }
   }

   public class NamedEventCreator
   {
      [DllImport("KERNEL32.DLL", EntryPoint = "CreateEventW", SetLastError = true)]
      private static extern SafeWaitHandle CreateEvent(
         IntPtr lpEventAttributes,
         bool bManualReset,
         bool bInitialState,
         string lpName);

      public static AutoResetEvent CreateAutoResetEvent(bool initialState, string name)
      {
         // Создать именованное событие
         SafeWaitHandle rawEvent = CreateEvent(IntPtr.Zero, false, initialState, name);
         if (rawEvent.IsInvalid)
         {
            throw new Win32Exception(Marshal.GetLastWin32Error());
         }
         // Создать событие управляемого типа на основе дескриптора
         var autoEvent = new AutoResetEvent(false) { SafeWaitHandle = rawEvent };
         // Очистить дескриптор, находящийся в autoEvent,
         // перед заменой его именованным дескриптором.
         return autoEvent;
      }
   }
}
