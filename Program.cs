using NXOpen;
using NXOpen.CAM;
using NXOpen.UF;
using System;

public class Program
{
    // class members
    private static Session theSession;
    private static UI theUI;
    private static UFSession theUfSession;
    public static Program theProgram;
    public static bool isDisposeCalled;

    //------------------------------------------------------------------------------
    // Constructor
    //------------------------------------------------------------------------------
    public Program()
    {
        try
        {
            theSession = Session.GetSession();
            theUI = UI.GetUI();
            theUfSession = UFSession.GetUFSession();
            isDisposeCalled = false;
        }
        catch (NXOpen.NXException ex)
        {
            // ---- Enter your exception handling code here -----
            // UI.GetUI().NXMessageBox.Show("Message", NXMessageBox.DialogType.Error, ex.Message);
        }
    }

    //------------------------------------------------------------------------------
    //  Explicit Activation
    //      This entry point is used to activate the application explicitly
    //------------------------------------------------------------------------------
    public static int Main(string[] args)
    {
        int retValue = 0;
        try
        {
            theProgram = new Program();
            //TODO: Add your application code here        
            NXOpen.Part workPart = theSession.Parts.Work;
            NXOpen.Part displayPart = theSession.Parts.Display;

            var feats = workPart.CAMFeatures.ToArray();

            foreach (var cAMFeature in feats)
            {
                // ��ȡ��������
                string featureName = cAMFeature.Name;

             
               //����1���жϿ׵ĳ�����
                  if (featureName.StartsWith("�ڿ�"))
                    {
                    Double FeaDia = cAMFeature.GetAttributeDoubleValue("DIAMETER_1");
                    Double FeaDepth = cAMFeature.GetAttributeDoubleValue("DEPTH");
                    Double ratio = FeaDepth / FeaDia;
                    if (ratio > 7)
                    {
                        var msg = $"{cAMFeature.Name} �׵ĳ�����Ϊ��{ratio.ToString("0.00")} ������7�����޸ġ�";

                        // Ϊ����������Ϳɫ
                        NXOpen.DisplayModification displayModification1;
                        displayModification1 = theSession.DisplayManager.NewDisplayModification();
                        displayModification1.ApplyToAllFaces = true;
                        displayModification1.ApplyToOwningParts = false;
                        displayModification1.NewColor = 186;
                        NXOpen.DisplayableObject[] objects1 = new NXOpen.DisplayableObject[1];
                        Face[] Faces = cAMFeature.GetFaces();
                        objects1 = Faces;
                        displayModification1.Apply(objects1);
                       
                        // �������
                        theSession.ListingWindow.Open();
                        theSession.ListingWindow.WriteLine(msg);
                    }
                }

                //����2���ж��˵��۵����̫С
                if (featureName.StartsWith("T�Ͳ�"))
                {
                    Double TFeaDepth = cAMFeature.GetAttributeDoubleValue("WIDTH_1");
    
                    if (TFeaDepth < 2.5)
                    {
                        var msg = $"{cAMFeature.Name} �����Ϊ{TFeaDepth .ToString("0.00")}��С��2.5mm�����޸ġ�";

                        // Ϊ����������Ϳɫ
                        NXOpen.DisplayModification displayModification1;
                        displayModification1 = theSession.DisplayManager.NewDisplayModification();
                        displayModification1.ApplyToAllFaces = true;
                        displayModification1.ApplyToOwningParts = false;
                        displayModification1.NewColor = 186;
                        NXOpen.DisplayableObject[] objects1 = new NXOpen.DisplayableObject[1];
                        Face[] Faces = cAMFeature.GetFaces();
                        objects1 = Faces;
                        displayModification1.Apply(objects1);

                        // �������
                        theSession.ListingWindow.Open();
                        theSession.ListingWindow.WriteLine(msg);
                    }
                }

                //����3���ж������ƴ󾶽�ģֱ���Ƿ���ȷ
                if (featureName.StartsWith("������"))
                {
                    Double Max_Dia = cAMFeature.GetAttributeDoubleValue("THREAD_MAJOR_DIAMETER");

                    if (Max_Dia != 20.15)
                    {
                        var msg = $"{cAMFeature.Name}  CAD��ģ��Ϊ{Max_Dia.ToString("0.00")}����ȷ�����M20*1.5��ȷ�Ĵ�Ϊ20.15mm�����޸ġ�";

                        // Ϊ����������Ϳɫ
                        NXOpen.DisplayModification displayModification1;
                        displayModification1 = theSession.DisplayManager.NewDisplayModification();
                        displayModification1.ApplyToAllFaces = true;
                        displayModification1.ApplyToOwningParts = false;
                        displayModification1.NewColor = 186;
                        NXOpen.DisplayableObject[] objects1 = new NXOpen.DisplayableObject[1];
                        Face[] Faces = cAMFeature.GetFaces();
                        objects1 = Faces;
                        displayModification1.Apply(objects1);

                        // �������
                        theSession.ListingWindow.Open();
                        theSession.ListingWindow.WriteLine(msg);
                    }
                }



                //����4���ж���Բ�۵ĵ���R���Ƿ�С��0.25
                if (featureName.StartsWith("��Բ��"))
                {
                    Double R_left = cAMFeature.GetAttributeDoubleValue("R_L");
    

                    if (R_left < 0.25)
                    {
                        var msg = $"{cAMFeature.Name}  �ײ�RΪ{R_left.ToString("0.00")} С��0.25mm�����޸ġ�";

                        // Ϊ����������Ϳɫ
                        NXOpen.DisplayModification displayModification1;
                        displayModification1 = theSession.DisplayManager.NewDisplayModification();
                        displayModification1.ApplyToAllFaces = true;
                        displayModification1.ApplyToOwningParts = false;
                        displayModification1.NewColor = 186;
                        NXOpen.DisplayableObject[] objects1 = new NXOpen.DisplayableObject[1];
                        Face[] Faces = cAMFeature.GetFaces();
                        objects1 = Faces;
                        displayModification1.Apply(objects1);

                        // �������
                        theSession.ListingWindow.Open();
                        theSession.ListingWindow.WriteLine(msg);
                    }
                }

            }
            theProgram.Dispose();
        }
        catch (NXOpen.NXException ex)
        {
            // ---- Enter your exception handling code here -----

        }
        return retValue;
    }

    //------------------------------------------------------------------------------
    // Following method disposes all the class members
    //------------------------------------------------------------------------------
    public void Dispose()
    {
        try
        {
            if (isDisposeCalled == false)
            {
                //TODO: Add your application code here 
            }
            isDisposeCalled = true;
        }
        catch (NXOpen.NXException ex)
        {
            // ---- Enter your exception handling code here -----

        }
    }

    public static int GetUnloadOption(string arg)
    {
        //Unloads the image explicitly, via an unload dialog
        //return System.Convert.ToInt32(Session.LibraryUnloadOption.Explicitly);

        //Unloads the image immediately after execution within NX
        return System.Convert.ToInt32(Session.LibraryUnloadOption.Immediately);

        //Unloads the image when the NX session terminates
        // return System.Convert.ToInt32(Session.LibraryUnloadOption.AtTermination);
    }

}

