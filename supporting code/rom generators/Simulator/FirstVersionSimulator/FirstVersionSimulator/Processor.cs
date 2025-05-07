using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class Processor
{
    public ushort[] registers = new ushort[16];
    /*
    0-r0
    1-r1
    2-r2
    3-r3
    4-r4
    5-r5
    6-r6
    7-r7
    8-pc
    9-sp
    10-stat
    11-ir
    12-MAR
    13-r13
    14-r14
    15-imr
     */
    ushort[] ram = new ushort[(int)Math.Pow(2, 16)];
    ushort aBus = 0;
    ushort bBus = 0;
    ushort cBus = 0;
    ushort data = 0;
    ushort address = 0;
    byte subInstructionCounter=0;
    UInt64[,] flags;
    string[] instructionNames = new string[256];
    ushort interuptStat = 0;

    ushort aluStat = 0;

    public Processor(string pathL, string pathH)
    {
        loadFile(pathL, pathH);
    }

    public Processor(ushort[] inputData)
    {
        loadFile(inputData);
    }





    public void resetProcessor()
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
        loadFlags();
        loadInstructionNames();
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

    public void loadFlags()
    {
        flags = new UInt64[256, 8];
        byte[] inputBytes=File.ReadAllBytes("C:\\Users\\breck\\OneDrive\\Documents\\_circuits\\16 bit computer\\github\\16BitComputer\\supporting code\\rom generators\\Simulator\\InstructionFlags\\instructionFlags.bin");
        for (int i = 0; i < 256; i++)
        {
            for (int c = 0; c < 8; c++)
            {
                for (int d = 0; d < 8; d++)
                {
                    flags[i,c] = flags[i,c] | (((UInt64)(inputBytes[i*8*8+c*8+d]))<<(d*8));
                }
                
            }
        }


    }

    public void loadInstructionNames()
    {

        string[] inputNames =File.ReadAllLines("C:\\Users\\breck\\OneDrive\\Documents\\_circuits\\16 bit computer\\github\\16BitComputer\\supporting code\\rom generators\\Simulator\\CreatingInstructionFlags\\CreatingInstructionFlags\\InstructionNames.csv");
        for (int i = 0; i < 256; i++)
        {
            if (i < inputNames.Length)
            {
                instructionNames[i] = inputNames[i];
            }
            else
            {
                instructionNames[i] = "";
            }

        }
    }


    public void printU64(UInt64 a)
    {
        string binValue = "";
        for (int d = 0; d < 64; d++)
        {
            binValue += (a >>> d) & 1;
        }
        Console.WriteLine(binValue);
    }

    public void risingClock()
    {
        int currentInstruction = registers[11] & 255;
        int rnA = (registers[11] & 0b11100000000)>>8;
        int rnB= (registers[11] & 0b11100000000000)>>11;
        if (subInstructionCounter == 1)
        {
            Console.WriteLine(instructionNames[currentInstruction]);
        }
        //bool reading = false;
        //rna to a
        if ((flags[currentInstruction, subInstructionCounter] & 1) != 0) { aBus = registers[rnA]; }//Console.Write("rna to a "); }
        //pc to a
        if ((flags[currentInstruction, subInstructionCounter] & (1<<1)) != 0) { aBus = registers[8]; }//Console.Write("pc to a "); }
        //rnb to b
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 2)) != 0) { bBus = registers[rnB]; }//Console.Write("rnb to b "); }
        //imr to b
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 3)) != 0) { bBus = registers[15]; }//Console.Write("imr to b "); }
        //c to rna
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 4)) != 0) {/*does stuff in falling*/ }//Console.Write("c to rna "); }
        //c to pc
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 5)) != 0) { /*does stuff in falling*/}//Console.Write("c to pc "); }
        //c to ir
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 6)) != 0) { /*does stuff in falling*/}//Console.Write("c to ir "); }
        //c to mar
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 7)) != 0) {/*does stuff in falling*/ }//Console.Write("c to mar "); }
        //c to imr
        if ((flags[currentInstruction, subInstructionCounter] & (1<<8)) != 0) { /*does stuff in falling*/}//Console.Write("c to imr "); }
        //a to c
        //bellow
        //b to c
        //bellow
        //alu to c 
        bool aluToC = false;
        if ((flags[currentInstruction, subInstructionCounter] & (1<<11)) != 0) { aluToC = true; }//Console.Write("alu to c "); }
        //a to d
        if ((flags[currentInstruction, subInstructionCounter] & (1<<12)) != 0) { data = aBus; }//Console.Write("a to d "); }
        //b to d
        if ((flags[currentInstruction, subInstructionCounter] & (1<<13)) != 0) { data = bBus; }//Console.Write("b to d "); }
        //d to c
        //bellow
        //inc pc
        if ((flags[currentInstruction, subInstructionCounter] & (1<<15)) != 0) { /*nothing until falling*/ }//Console.Write("inc pc "); }
        //inc sp
        if ((flags[currentInstruction, subInstructionCounter] & (1<< 16)) != 0) { /*nothing until falling*/ }//Console.Write("inc sp "); }
        //dec sp
        if ((flags[currentInstruction, subInstructionCounter] & (1<< 17)) != 0) { /*nothing until falling*/ }//Console.Write("dec sp "); }
        //pc to address
        if ((flags[currentInstruction, subInstructionCounter] & (1<< 18)) != 0) { address = registers[8]; }//Console.Write("pc to address "); }
        //mar to address
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 19)) != 0) { address = registers[12]; }//Console.Write("mar to address "); }
        //sp to address 
        if ((flags[currentInstruction, subInstructionCounter] & (1<< 20)) != 0) { address = registers[9]; }//Console.Write("sp to address"); }
        //alu to stat
        if ((flags[currentInstruction, subInstructionCounter] & (1<< 21)) != 0) { /*nothing until falling*/}//Console.Write("alu to stat "); }
        //c to pcc
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 22)) != 0) { /*nothing until falling*/}//Console.Write("c to pcc "); }
        //reset stat
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 23)) != 0) { /*nothing until falling*/ }//Console.Write("reset stat "); }
        //end
        if ((flags[currentInstruction, subInstructionCounter] & (1<< 24)) != 0) { /*nothing until falling*/}//Console.Write("end "); }
        //pc to a
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 25)) != 0) { aBus = registers[8]; }//Console.Write("pc to a "); }



        //putting stuff to c
        if (aluToC)
        {
            cBus = alu((ushort)currentInstruction);
        }

        //a to c
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 9)) != 0) { cBus = aBus; }//Console.Write("a to c "); }
        //b to c
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 10)) != 0) { cBus = bBus; }//Console.Write("b to c "); }
        //d to c
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 14)) != 0) { data=ram[address]; }//Console.Write("d to c "); }


     //   Console.WriteLine(instructionNames[registers[11]&255]+" "+rnA+" "+rnB);
    }
    public void fallingClock()
    {

        int currentInstruction = registers[11] & 255;
        int rnA = (registers[11] & 0b11100000000) >> 8;
        int rnB = (registers[11] & 0b11100000000000) >> 11;


        //a to c
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 9)) != 0) { cBus = aBus; }
        //b to c
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 10)) != 0) { cBus = aBus; }
        //d to c
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 14)) != 0) { cBus = data; }

        //rna to a
        if ((flags[currentInstruction, subInstructionCounter] & 1) != 0) { /*does stuff in rising*/ }
        //pc to a
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 1)) != 0) { /*does stuff in rising*/ }
        //rnb to b
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 2)) != 0) { /*does stuff in rising*/ }
        //imr to b
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 3)) != 0) { /*does stuff in rising*/ }
        //c to rna
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 4)) != 0) { registers[rnA] = cBus; }
        //c to pc
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 5)) != 0) { registers[8] = cBus; }
        //c to ir
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 6)) != 0) { registers[11] = cBus; }
        //c to mar
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 7)) != 0) { registers[12] = cBus; }
        //c to imr
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 8)) != 0) { registers[15] = cBus; }
        //alu to c 
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 11)) != 0) { /*does stuff in rising*/  }
        //a to d
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 12)) != 0) { ram[address] = data; }
        //b to d
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 13)) != 0) { ram[address] = data; }
        //inc pc
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 15)) != 0) { registers[8]++; }
        //inc sp
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 16)) != 0) {  registers[9]++; registers[9] = (ushort)((int)registers[9] & 255); }
        //dec sp
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 17)) != 0) { registers[9]++; registers[9] = (ushort)((int)registers[9] & 255); }
        //pc to address
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 18)) != 0) { /*does stuff in rising*/ }
        //mar to address
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 19)) != 0) { /*does stuff in rising*/}
        //sp to address 
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 20)) != 0) { /*does stuff in rising*/}
        //alu to stat
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 21)) != 0) { registers[10]= aluStat; }
        //c to pcc
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 22)) != 0) {
            if (conditionMet(registers[10], (ushort)currentInstruction))
            {
                registers[8] = cBus;
            }
            else {
                registers[8]++;
            }
        }
        //reset stat
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 23)) != 0) { registers[10] = interuptStat; }
        //end
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 24)) != 0) { subInstructionCounter = 0; } else
        {
            subInstructionCounter++;
        }
        //pc to a
        if ((flags[currentInstruction, subInstructionCounter] & (1 << 25)) != 0) {/*does stuff in rising*/ }


    }

   


    public bool conditionMet(ushort status, ushort instruction)
    {
        instruction=(ushort)(instruction & 0b1111);

        byte[,] importantBitsAndConditionBits =
        {

            //iren,n,z,c
            {0b0000,0b0000},//jmp-does not matter
            {0b0000,0b0000},//jmp-does not matter
            {0b0000,0b0000},//jsr-does not matter
            {0b0000,0b0000},//jsr-does not matter

            {0b0000,0b0000},//bra
            {0b0010,0b0010},//brz
            {0b0010,0b0000},//brnz
            {0b0100,0b0100},//brn

            {0b0100,0b0000},//brp
            {0b0110,0b0110},//brzn
            {0b0110,0b0010},//brzp
            {0b0001,0b0001},//brc

            {0b0001,0b0000},//brnc
            {0b0000,0b0000},//ret-does not matter
            {0b0000,0b0000},//rti-does not matter
            {0b0000,0b0000},//nothing
        };

        for (int i = 0; i < 4; i++)
        {
            //this bit matters
            if ((importantBitsAndConditionBits[instruction, 0] & (1 << i)) == 1)
            {
                if ((importantBitsAndConditionBits[instruction, 1] & (1 << i))== (status& (1 << i)))
                {
                    return true;
                }

            }
        }

        return true;
    }


    public ushort alu(ushort instruction)
    {
        int returnValue=0;

        //add
        if (instruction >> 2 == 0) { returnValue = aBus + bBus; }// Console.WriteLine("add: "+aBus+"+"+bBus+"="+returnValue); }
        //adc
        if (instruction >> 2 == 1) { returnValue = aBus + bBus + (registers[10]&1); }
        //sub
        if (instruction >> 2 == 2) { returnValue = aBus - bBus; }
        //sbc
        if (instruction >> 2 == 3) { returnValue = aBus - bBus+ (registers[10] & 1); }//might be wrong
        //and
        if (instruction >> 2 == 4) { returnValue = aBus & bBus; }
        //or
        if (instruction >> 2 == 5) { returnValue = aBus | bBus; }
        //nand
        if (instruction >> 2 == 6) { returnValue = ~(aBus & bBus); }
        //nor
        if (instruction >> 2 == 7) { returnValue = ~(aBus | bBus); }
        //xor
        if (instruction >> 2 == 8) { returnValue = aBus ^ bBus; }
        //xnor
        if (instruction >> 2 == 9) { returnValue = ~(aBus ^ bBus); }
        //cmp
        if (instruction >> 2 == 10) { returnValue = aBus - bBus; }

        //shr
        if (instruction >> 1 == 22) { returnValue = aBus >>> bBus; }
        //ror
        if (instruction >> 1 == 23) { returnValue = (aBus >>> bBus)|(aBus<<(16-bBus)); }
        //shl
        if (instruction >> 1 == 24) { returnValue = aBus << bBus; }
        //rol
        if (instruction >> 1 == 25) { returnValue = (aBus << bBus) | (aBus >> (16 - bBus)); }



        aluStat = 0;
        //set negative
        if (returnValue < 0)
        {
            aluStat = (ushort)(aluStat | 4);
        }
        //set zero
        if (returnValue < 0)
        {
            aluStat = (ushort)(aluStat | 2);
        }
        //set carry
        if ((returnValue & (~0xFFFF)) != 0)
        {
            aluStat =(ushort) (aluStat | 1);
        }

        return (ushort)(returnValue);
    }

}



