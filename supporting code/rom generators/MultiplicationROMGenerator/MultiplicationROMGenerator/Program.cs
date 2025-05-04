

using System.IO;
using System.Text;

Console.WriteLine("Hello, World!");


int bitWidth = 4;
byte[] output = new byte[(int)(Math.Pow(2, bitWidth) * Math.Pow(2, bitWidth))];




for (int i = 0; i < Math.Pow(2,bitWidth); i++)
{
    for (int c = 0; c < Math.Pow(2, bitWidth); c++)
    {
        output[(int)(c + i * Math.Pow(2, bitWidth))] = (byte)(i * c);
    }
}

// Build Logisim ROM text
StringBuilder sb = new StringBuilder();
sb.AppendLine("v2.0 raw");

for (int i = 0; i < output.Length; i++)
{
    sb.AppendFormat("{0:X2} ", output[i]);

    // Optional: break every 16 bytes
    if ((i + 1) % 16 == 0)
        sb.AppendLine();
}

//File.WriteAllBytes("C:\\Users\\breck\\Documents\\_CIRCUITS AND 6502\\16 bit computer github\\16BitComputer\\logisim simulations\\v0\\ROMs\\4x4BitMultROM", output);
File.WriteAllText("C:\\Users\\breck\\Documents\\_CIRCUITS AND 6502\\16 bit computer github\\16BitComputer\\logisim simulations\\v0\\ROMs\\4x4BitMultROM", sb.ToString());