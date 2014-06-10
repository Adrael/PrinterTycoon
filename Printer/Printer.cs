using System;
using System.Collections.Generic;

namespace Printer
{
    public class Printer
    {
        private String printerName;
        public PrinterState printerState;
        // pages per minute
        private int printingSpeed;
        private List<PrinterJob> jobsToPrint = new List<PrinterJob>();
        private PrinterJob currentlyPrintingJob = null;
        private float printProgression = 0f;

        public Printer()
        {
            printerName = "imprimante_";
            printerState = PrinterState.OFF;
        }

        public String getPrinterName()
        {
            return printerName;
        }

        public PrinterState getCurrentPrinterState()
        {
            return printerState;
        }

        public override string ToString()
        {
            return printerName;
        }

        public void setName(string text)
        {
            this.printerName = text;
        }

        public void setSpeed(int value)
        {
            this.printingSpeed = value;
        }

        public long timeToPrint(long sizeOfFileToPrint)
        {
            long totalSize = sizeOfFileToPrint;
            foreach (PrinterJob job in jobsToPrint)
            {
                totalSize += job.getFileSize();
            }

            if (currentlyPrintingJob != null)
            {
                totalSize += (long)(currentlyPrintingJob.getFileSize() * printProgression / 100f);
            }

            return totalSize / (printingSpeed * 100);
        }

        internal class PrinterJob
        {
            private String jobsName;
            // size of file in byte
            private long fileSize;

            public PrinterJob(String jobsName, long fileSize)
            {
                this.jobsName = jobsName;
                this.fileSize = fileSize;
            }

            public String getJobsName()
            {
                return jobsName;
            }

            public long getFileSize()
            {
                return fileSize;
            }

        }
    }
}
