using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hartalega.BarcodeScannerIntegrator
{
    public class D365ClientConfiguration
    {

        private static D365ClientConfiguration _defult;
        public static D365ClientConfiguration Default //{ get; set; }
        {
            get
            {
                if (_defult == null)
                {
                    _defult = D365ClientConfiguration.OneBox;
                }
                return _defult;
            }
        }
        
        public string UriString { get; set; }

        public bool UseWebAppAuthentication { get; set; }

        public bool ValidateAuthority { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string ActiveDirectoryResource { get; set; }
        public String ActiveDirectoryTenant { get; set; }
        public String ActiveDirectoryClientAppId { get; set; }
        public string ActiveDirectoryClientAppSecret { get; set; }
        public string DataAreaId { get; set; }

        public D365ClientConfiguration()
        {
        }

        public static D365ClientConfiguration OneBox = new D365ClientConfiguration()
        {
            ValidateAuthority = true,
            UseWebAppAuthentication = true,
            UriString = "https://usnconeboxax1aos.cloud.onebox.dynamics.com/",
            UserName = "htlgtest1@9dots.com",
            Password = "Hello9Dots",
            ActiveDirectoryResource = "https://usnconeboxax1aos.cloud.onebox.dynamics.com",
            ActiveDirectoryTenant = "https://login.windows.net/611d3a5a-d305-4b4c-aba7-2cacd9925c6c",
            ActiveDirectoryClientAppId = "6eaeff7f-6180-442d-a7b7-dfa4bf852e32",
            ActiveDirectoryClientAppSecret = "7vVFA4fj6oeHo4pmIS7Da5aXECH6GKHDCF3os1nQwdQ=",
            DataAreaId = "HNGC"
        };

        /// <summary>
        /// For add hoc run Unit Test for On-Premise environment only
        /// UAT & Production environment will get setting from database
        /// Auto build process always on Development box(use OneBox setting above)
        /// </summary>
        public static D365ClientConfiguration UATOnPremise = new D365ClientConfiguration()
        {
            ValidateAuthority = false,
            UseWebAppAuthentication = true,
            //Note that AOS config XML is on AOS machines in: C:\ProgramData\SF\AOS_10\Fabric\work\Applications\AXSFType_App84\AXSF.Package.1.0.xml
            UriString = "https://ax.d365uat.hartalega.com.my/namespaces/AXSF/",
            UserName = "",
            Password = "",
            ActiveDirectoryResource = "https://ax.d365uat.hartalega.com.my/",//this is the value for AADValidAudience from the AOS config xml
            ActiveDirectoryTenant = "https://adfs.hartalega.com.my/adfs",//this is the value for AADIssuerNameFormat (minus the placeholder {0}, instead suffix "/adfs") from AOS config xml
            ActiveDirectoryClientAppId = "0b012c30-1161-4454-8bb3-313690e598ae",//client app ID is from ADFS management - configure a application group
            ActiveDirectoryClientAppSecret = "1kBH8oSeQSuMJHvW-xxnVkD2XNNH2Dt83FmyWfI-",//secret is from ADFS management - same place as the client app ID
            DataAreaId = "HNGC"
        };


        /// <summary>
        /// For add hoc run Unit Test for On-Premise environment only
        /// UAT & Production environment will get setting from database
        /// Auto build process always on Development box(use OneBox setting above)
        /// </summary>
        public static D365ClientConfiguration D365LiveOnPremise = new D365ClientConfiguration()
        {
            ValidateAuthority = false,
            UseWebAppAuthentication = true,
            //Note that AOS config XML is on AOS machines in: C:\ProgramData\SF\AOS_10\Fabric\work\Applications\AXSFType_App84\AXSF.Package.1.0.xml
            UriString = "https://ax.d365live.hartalega.com.my/namespaces/AXSF/",
            UserName = "",
            Password = "",
            ActiveDirectoryResource = "https://ax.d365live.hartalega.com.my",//this is the value for AADValidAudience from the AOS config xml
            ActiveDirectoryTenant = "https://adfs.hartalega.com.my/adfs",//this is the value for AADIssuerNameFormat (minus the placeholder {0}, instead suffix "/adfs") from AOS config xml
            ActiveDirectoryClientAppId = "7334ad95-edc2-4221-852a-eaa8e1cad193",//client app ID is from ADFS management - configure a application group
            ActiveDirectoryClientAppSecret = "TuYPVkftKv3Sk9hfg4Yx3RoJWHvtCiYNuhutt_57",//secret is from ADFS management - same place as the client app ID
        };

        public static D365ClientConfiguration UAT_V10 = new D365ClientConfiguration()
        {
            ValidateAuthority = false,
            UseWebAppAuthentication = true,
            //Note that AOS config XML is on AOS machines in: C:\ProgramData\SF\AOS_10\Fabric\work\Applications\AXSFType_App84\AXSF.Package.1.0.xml
            UriString = "https://ax-d365.hartalega.com.my/namespaces/AXSF/",
            UserName = "",
            Password = "",
            ActiveDirectoryResource = "https://ax-d365.hartalega.com.my",//this is the value for AADValidAudience from the AOS config xml
            ActiveDirectoryTenant = "https://adfs.hartalega.com.my/adfs",//this is the value for AADIssuerNameFormat (minus the placeholder {0}, instead suffix "/adfs") from AOS config xml
            ActiveDirectoryClientAppId = "9977ec68-630f-419b-ad39-20e42872a3d5",//client app ID is from ADFS management - configure a application group
            ActiveDirectoryClientAppSecret = "ps0J3FBGeIWMUXERd_CpCgtwYCrbRd18rl8NFrIV",//secret is from ADFS management - same place as the client app ID
            DataAreaId = "HNGC"
        };

        /// <summary>
        /// For add hoc run Unit Test for On-Premise environment only
        /// UAT & Production environment will get setting from database
        /// Auto build process always on Development box(use OneBox setting above)
        /// </summary>
        public static D365ClientConfiguration D365UAT_HSB = new D365ClientConfiguration()
        {
            ValidateAuthority = false,
            UseWebAppAuthentication = true,
            //Note that AOS config XML is on AOS machines in: C:\ProgramData\SF\AOS_10\Fabric\work\Applications\AXSFType_App84\AXSF.Package.1.0.xml
            UriString = "https://ax-hsbd365uat.hartalega.com.my/namespaces/AXSF/",
            UserName = "",
            Password = "",
            ActiveDirectoryResource = "https://ax-hsbd365uat.hartalega.com.my",//this is the value for AADValidAudience from the AOS config xml
            ActiveDirectoryTenant = "https://adfshsbd365.hartalega.com.my/adfs",//this is the value for AADIssuerNameFormat (minus the placeholder {0}, instead suffix "/adfs") from AOS config xml
            ActiveDirectoryClientAppId = "022b7490-5e1a-499f-b448-de7b35e7500d",//client app ID is from ADFS management - configure a application group
            ActiveDirectoryClientAppSecret = "Qsfr_lev77I3GG4WdQmllWS8Du2WfPVbC59StxBA",//secret is from ADFS management - same place as the client app ID
            DataAreaId = "HSB"
        };



        /// <summary>
        /// For add hoc run Unit Test for On-Premise environment only
        /// UAT & Production environment will get setting from database
        /// Auto build process always on Development box(use OneBox setting above)
        /// </summary>
        public static D365ClientConfiguration D365_HSB_Live_OnPremise = new D365ClientConfiguration()
        {
            ValidateAuthority = false,
            UseWebAppAuthentication = true,
            //Note that AOS config XML is on AOS machines in: C:\ProgramData\SF\AOS_10\Fabric\work\Applications\AXSFType_App84\AXSF.Package.1.0.xml
            UriString = "https://ax-hsbd365live.hartalega.com.my/namespaces/AXSF/",
            UserName = "",
            Password = "",
            ActiveDirectoryResource = "https://ax-hsbd365live.hartalega.com.my",//this is the value for AADValidAudience from the AOS config xml
            ActiveDirectoryTenant = "https://adfshsbd365.hartalega.com.my/adfs",//this is the value for AADIssuerNameFormat (minus the placeholder {0}, instead suffix "/adfs") from AOS config xml
            ActiveDirectoryClientAppId = "0a194070-9cfe-42da-8ab7-966685f49fb6",//client app ID is from ADFS management - configure a application group
            ActiveDirectoryClientAppSecret = "aSYVtN9Lzj7I7lEnNtOVuUDl0Ac4kCh4MTRIx_ar",//secret is from ADFS management - same place as the client app ID
        };
    }
}
