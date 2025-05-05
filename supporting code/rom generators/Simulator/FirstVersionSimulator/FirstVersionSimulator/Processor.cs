using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Processor
{
    ushort[] registers = new ushort[16];
    ushort[] ram = new ushort[(int)Math.Pow(2, 16)];
    ushort aBus = 0;
    ushort bBus = 0;
    ushort cBus = 0;
    ushort data = 0;
    ushort address = 0;
    byte subInstructionCounter=0;
    UInt64 flags = 0;
   

    public Processor(string pathL, string pathH)
    {
        loadFile(pathL, pathH);
    }

    public Processor(ushort[] inputData)
    {
        loadFile(inputData);
    }





    void resetProcessor()
    {
        for (int i = 0; i < registers.Length; i++)
        {
            registers[i] = 0;
        }
        aBus = 0;
        bBus = 0;
        cBus = 0;
        data = 0;
        address = 0;
        subInstructionCounter = 0;
        flags = 0;
    }




    public void loadFile(string pathL, string pathH)
    {
        byte[] bytesL = File.ReadAllBytes(pathL);
        byte[] bytesH = File.ReadAllBytes(pathH);
        if (bytesL.Length != bytesH.Length)
        {
            Console.WriteLine("File sizes do not match");
        }
        else
        {
            for (int i = 0; i < Math.Min(bytesL.Length, ram.Length); i++)
            {
                ram[i] = (ushort)((int)(bytesL[i]) + ((int)bytesH[i]) << 8);
            }
        }
    }

    public void loadFile(ushort[] inputData)
    {

        for (int i = 0; i < Math.Min(inputData.Length, ram.Length); i++)
        {
            ram[i] = inputData[i];
        }

    }


    public void risingClock()
    {
        flags = 0;
        if (subInstructionCounter == 0)
        { 
            
        }
    }
    private void fallingClock()
    {

    }

}



