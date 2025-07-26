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
                // 获取特征名称
                string featureName = cAMFeature.Name;

             
               //规则1：判断孔的长径比
                  if (featureName.StartsWith("内孔"))
                    {
                    Double FeaDia = cAMFeature.GetAttributeDoubleValue("DIAMETER_1");
                    Double FeaDepth = cAMFeature.GetAttributeDoubleValue("DEPTH");
                    Double ratio = FeaDepth / FeaDia;
                    if (ratio > 7)
                    {
                        var msg = $"{cAMFeature.Name} 孔的长径比为：{ratio.ToString("0.00")} ，大于7，请修改。";

                        // 为有问题特征涂色
                        NXOpen.DisplayModification displayModification1;
                        displayModification1 = theSession.DisplayManager.NewDisplayModification();
                        displayModification1.ApplyToAllFaces = true;
                        displayModification1.ApplyToOwningParts = false;
                        displayModification1.NewColor = 186;
                        NXOpen.DisplayableObject[] objects1 = new NXOpen.DisplayableObject[1];
                        Face[] Faces = cAMFeature.GetFaces();
                        objects1 = Faces;
                        displayModification1.Apply(objects1);
                       
                        // 输出报告
                        theSession.ListingWindow.Open();
                        theSession.ListingWindow.WriteLine(msg);
                    }
                }

                //规则2：判断退刀槽的深度太小
                if (featureName.StartsWith("T型槽"))
                {
                    Double TFeaDepth = cAMFeature.GetAttributeDoubleValue("WIDTH_1");
    
                    if (TFeaDepth < 2.5)
                    {
                        var msg = $"{cAMFeature.Name} 的深度为{TFeaDepth .ToString("0.00")}，小于2.5mm，请修改。";

                        // 为有问题特征涂色
                        NXOpen.DisplayModification displayModification1;
                        displayModification1 = theSession.DisplayManager.NewDisplayModification();
                        displayModification1.ApplyToAllFaces = true;
                        displayModification1.ApplyToOwningParts = false;
                        displayModification1.NewColor = 186;
                        NXOpen.DisplayableObject[] objects1 = new NXOpen.DisplayableObject[1];
                        Face[] Faces = cAMFeature.GetFaces();
                        objects1 = Faces;
                        displayModification1.Apply(objects1);

                        // 输出报告
                        theSession.ListingWindow.Open();
                        theSession.ListingWindow.WriteLine(msg);
                    }
                }

                //规则3：判断外螺纹大径建模直径是否正确
                if (featureName.StartsWith("外螺纹"))
                {
                    Double Max_Dia = cAMFeature.GetAttributeDoubleValue("THREAD_MAJOR_DIAMETER");

                    if (Max_Dia != 20.15)
                    {
                        var msg = $"{cAMFeature.Name}  CAD建模大径为{Max_Dia.ToString("0.00")}不正确，规格M20*1.5正确的大径为20.15mm，请修改。";

                        // 为有问题特征涂色
                        NXOpen.DisplayModification displayModification1;
                        displayModification1 = theSession.DisplayManager.NewDisplayModification();
                        displayModification1.ApplyToAllFaces = true;
                        displayModification1.ApplyToOwningParts = false;
                        displayModification1.NewColor = 186;
                        NXOpen.DisplayableObject[] objects1 = new NXOpen.DisplayableObject[1];
                        Face[] Faces = cAMFeature.GetFaces();
                        objects1 = Faces;
                        displayModification1.Apply(objects1);

                        // 输出报告
                        theSession.ListingWindow.Open();
                        theSession.ListingWindow.WriteLine(msg);
                    }
                }



                //规则4：判断外圆槽的底面R角是否小于0.25
                if (featureName.StartsWith("外圆槽"))
                {
                    Double R_left = cAMFeature.GetAttributeDoubleValue("R_L");
    

                    if (R_left < 0.25)
                    {
                        var msg = $"{cAMFeature.Name}  底部R为{R_left.ToString("0.00")} 小于0.25mm，请修改。";

                        // 为有问题特征涂色
                        NXOpen.DisplayModification displayModification1;
                        displayModification1 = theSession.DisplayManager.NewDisplayModification();
                        displayModification1.ApplyToAllFaces = true;
                        displayModification1.ApplyToOwningParts = false;
                        displayModification1.NewColor = 186;
                        NXOpen.DisplayableObject[] objects1 = new NXOpen.DisplayableObject[1];
                        Face[] Faces = cAMFeature.GetFaces();
                        objects1 = Faces;
                        displayModification1.Apply(objects1);

                        // 输出报告
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

