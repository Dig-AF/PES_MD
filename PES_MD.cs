using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;

namespace EAWS.Core.SilverBullet
{
    public static class PES_MD
    {
        static string[][] Element_Lookup = new string[][] { 
                            new string[] { "Activity", "Activity (DM2)", "IndividualType", "1154", "1326", "default" }, 
                            new string[] { "Activity", "Operational Activity", "IndividualType", "", "", "extra" },
                            new string[] { "Activity", "Project Milestone (DM2x)", "IndividualType", "", "", "extra" },
                            new string[] { "Activity", "System Milestone (DM2x)", "IndividualType", "", "", "extra" },
                            new string[] { "Capability", "Capability (DM2)", "IndividualType", "1155", "1327", "default" },
                            new string[] { "Performer", "Performer (DM2)", "IndividualType", "1178", "1367", "default" },
                            new string[] { "Activity", "System Function (DM2x)", "IndividualType", "1207", "1384", "extra" },
                            new string[] { "Activity", "Service Function (DM2x)", "IndividualType", "1207", "1395", "extra" },
                            new string[] { "Activity", "Event (DM2x)", "IndividualType", "1207", "1463", "extra" },
                            new string[] { "Service", "Service (DM2)", "IndividualType","1160", "1376", "default" },
                            new string[] { "Resource", "Resource (DM2)", "IndividualType","", "", "default" },
                            new string[] { "System", "System (DM2)", "IndividualType","1188", "1377", "default" },
                            new string[] { "System", "Data Store (DM2x)", "IndividualType","1214", "1385", "extra" },
                            new string[] { "Materiel", "Materiel (DM2)", "IndividualType","1177", "1366", "default" },
                            new string[] { "Information", "Information (DM2)", "IndividualType","1176", "1365", "default" },
                            new string[] { "PersonRole", "Person (DM2)", "IndividualType","1186", "1375", "default" },
                            new string[] { "DomainInformation", "DomainInformation (DM2)", "IndividualType","", "1371", "default" },

                            new string[] { "Data", "Table", "IndividualType","", "1370", "extra" },
                            new string[] { "Data", "Column", "IndividualType","", "1370", "extra" },
                            new string[] { "Data", "Index", "IndividualType","", "1370", "extra" },
                            new string[] { "Data", "Physical Foreign Key Component", "IndividualType","", "1370", "extra" },

                            new string[] { "Data", "Data (DM2)", "IndividualType","", "1370", "default" },
                            new string[] { "Data", "Entity", "IndividualType","30", "15", "extra" },
                            new string[] { "Data", "Attribute", "IndividualType","30", "5", "extra" },
                            new string[] { "Data", "Data Element", "IndividualType","30", "105", "extra" },
                            new string[] { "Data", "Access Path", "IndividualType","21", "21", "extra" },
                            new string[] { "Performer", "ServiceInterface (DM2)", "IndividualType","", "", "extra" },
                            new string[] { "Performer", "Interface (Port) (DM2)", "IndividualType","", "", "extra" },
                            new string[] { "OrganizationType", "Organization (DM2)", "IndividualType","1185", "1374", "default" },
                            new string[] { "Condition", "Condition (Environmental) (DM2)", "IndividualType","1156", "1328", "default" },
                            new string[] { "Location", "Location (DM2)", "IndividualType","1161", "", "default" },
                            new string[] { "RegionOfCountry", "RegionOfCountry (DM2)", "IndividualType","1357", "1357", "default" },
                            new string[] { "Country", "Country (DM2)", "IndividualType","1358", "1358", "default" },
                            new string[] { "Rule", "Rule (DM2)", "IndividualType","1173", "1362", "default" },

                            //new string[] { "Rule", "Constraint", "IndividualType","1173", "1362", "extra" },

                            new string[] { "IndividualType", "IndividualType", "IndividualType","", "", "default" },
                            new string[] { "ArchitecturalDescription", "ArchitecturalDescription (DM2)", "IndividualType","1179", "1368","default" },
                            new string[] { "ServiceDescription", "ServiceDescription (DM2)", "IndividualType","", "1369","default" },
                            new string[] { "ProjectType", "Project (DM2)", "IndividualType","1159", "1348","default" },
                            new string[] { "Vision", "Vision (DM2)", "IndividualType","1172", "1361","default" },
                            new string[] { "Guidance", "Guidance (DM2)", "IndividualType","1157", "1329","default" },
                            new string[] { "Facility", "Facility (DM2)", "IndividualType","", "1353","default" },
                            new string[] { "RealProperty", "RealProperty (DM2)", "IndividualType","", "1352","default" },
                            new string[] { "Site", "Site (DM2)", "IndividualType","", "1354","default" },
                            };

        static string[][] RSA_Element_Lookup = new string[][] { 
                            new string[] { "Activity", "OperationalNodeSpecification", "IndividualType", "", "", "default" },
                            new string[] { "Capability", "Capability", "IndividualType", "", "", "default" },
                            new string[] { "System", "System", "IndividualType", "", "", "default" },

                            };

        static string[][] Tuple_Lookup = new string[][] { 
                            };

        static string[][] Tuple_Type_Lookup = new string[][] { 
                            new string[] { "WholePartType", "Data Element", "WholePartType", "1", "Attribute", "Attribute" },
                            new string[] { "WholePartType", "Table Name", "WholePartType", "2", "Attribute", "Attribute" },
                            
                            new string[] { "WholePartType", "Foreign Keys and Roles", "WholePartType", "1", "Attribute", "Attribute" },
                            new string[] { "WholePartType", "Constraint Name", "WholePartType", "2", "Attribute", "Attribute" },

                            new string[] { "WholePartType", "Primary Index Name", "WholePartType", "1", "Element", "Element" },
                            //new string[] { "WholePartType", "Primary Key Name", "WholePartType", "2", "Element", "Element" },
                            new string[] { "WholePartType", "Entity Name", "WholePartType", "2", "Attribute", "Attribute" },
                            new string[] { "WholePartType", "activityParentOfActivity", "WholePartType", "1", "Activity (DM2)", "Activity" },
                            new string[] { "WholePartType", "activityPartOfActivity", "WholePartType", "2", "Activity (DM2)", "Activity" },
                            new string[] { "WholePartType", "Parent Of Capability", "WholePartType", "1", "Capability (DM2)", "Capability" },
                            new string[] { "WholePartType", "Parent Capability", "WholePartType", "2", "Capability (DM2)", "Capability" },
                            new string[] { "WholePartType", "Parent of Function", "WholePartType", "1", "Activity (DM2)", "Activity" },
                            new string[] { "WholePartType", "Part of Function", "WholePartType", "2", "Activity (DM2)", "Activity" },
                            new string[] { "WholePartType", "Parent Of Service", "WholePartType", "1", "Service (DM2)", "Service" },
                            new string[] { "WholePartType", "Part of Service", "WholePartType", "2", "Service (DM2)", "Service" },
                            new string[] { "WholePartType", "Parent of System", "WholePartType", "1", "System (DM2)", "System" }, 
                            new string[] { "WholePartType", "Part of System", "WholePartType", "2", "System (DM2)", "System" }, 
                            new string[] { "WholePartType", "PerformerMembers", "WholePartType", "1", "Organization (DM2)", "OrganizationType" },
                            new string[] { "WholePartType", "MemberOfOrganizations", "WholePartType", "2", "Organization (DM2)", "OrganizationType" },
                            new string[] { "WholePartType", "personPartOfPerformer", "WholePartType", "1", "Performer (DM2)", "Performer" },
                            new string[] { "WholePartType", "personPartOfPerformer", "WholePartType", "2", "Person (DM2)", "PersonRole" }, 
                            new string[] { "WholePartType", "portPartOfPerformer", "WholePartType", "5", "System (DM2)", "System" },
                            new string[] { "WholePartType", "portPartOfPerformer", "WholePartType", "4", "Interface (Port) (DM2)", "Performer" },
                            new string[] { "activityPartOfProjectType", "activityPartOfProjectType", "WholePartType", "5", "Project (DM2)", "ProjectType" },
                            new string[] { "activityPartOfProjectType", "activityPartOfProjectType", "WholePartType", "4", "Activity (DM2)", "Activity" },
                            new string[] { "WholePartType", "interfacePartOfService", "WholePartType", "5", "Service (DM2)", "Service" },
                            new string[] { "WholePartType", "interfacePartOfService", "WholePartType", "4", "ServiceInterface (DM2)", "Performer" },
                            //new string[] { "activityPartOfProjectType", "Milestones", "WholePartType", "1", "Activity (DM2)", "Activity" },
                            //new string[] { "activityPartOfProjectType", "Project", "WholePartType", "2", "Project (DM2)", "ProjectType" },
                            new string[] { "SupportedBy", "SupportedBy", "SupportedBy", "3", "Organization (DM2)", "OrganizationType" },
                            new string[] { "ruleConstrainsActivity", "ruleConstrainsActivity", "CoupleType", "2", "Activity (DM2)", "Activity" },
                            };

        static string[][] MD_Element_Lookup = new string[][] {
                            // DM2 Class, UPDM_Profile Element
                            //new string[] { "Activity", "OperationalMessage", "IndividualType", "base_Message" },
                            new string[] { "System", "System", "IndividualType", "base_Class" },
                            new string[] { "Service", "ServiceAccess", "IndividualType", "base_Class" },
                            new string[] { "Activity", "OperationalActivity", "IndividualType", "base_Activity" },
                            new string[] { "Performer", "Performer", "IndividualType", "base_Class" },
                            new string[] { "Performer", "ConceptRole", "IndividualType", "base_Property" },
                            new string[] { "Activity", "Function", "IndividualType", "base_Activity" },
                            new string[] { "Activity", "ActualProjectMilestone", "IndividualType", "base_InstanceSpecification" }, // revisit activityPartOfProject
                            new string[] { "Capability", "Capability", "IndividualType", "base_Class" },
                            new string[] { "PersonRole", "PersonType", "IndividualType", "base_Class" },
                            ////new string[] { "Project", "VersionOfConfiguration", "Individual" }, // revisit subType of Project
                            new string[] { "Project", "Project", "Individual", "base_InstanceSpecification" }, // revisit ProjectType?
                            new string[] { "ProjectType", "ProjectType", "IndividualType", "base_Class" }, // revisit ProjectType?
                            new string[] { "System", "CapabilityConfiguration", "IndividualType", "base_Class" }, // revisit subType of Capability
                            new string[] { "Activity", "ProjectMilestone", "IndividualType", "base_Class" }, // revisit subType of Project
                            new string[] { "Vision", "Vision", "IndividualType", "base_Class" },
                            new string[] { "MeasureType", "MeasureType", "IndividualTypeType", "base_DataType" },
                            new string[] { "Measure", "Measurement", "IndividualType", "base_Property" },
                            new string[] { "Resource", "ResourceInteraction", "IndividualType", "base_InformationFlow" }, // revisit
                            new string[] { "Resource", "ExchangeElement", "IndividualType", "base_Class" }, // revisit
                            new string[] { "Data", "LogicalDataModel", "IndividualType", "base_Package" },
                            new string[] { "Data", "EntityItem", "IndividualType", "base_Class" },
                            new string[] { "Data", "EntityAttribute", "IndividualType", "base_Property" },
                            new string[] { "Activity", "IncrementMilestone", "IndividualType", "base_InstanceSpecification" },
                            //new string[] { "Activity", "OperationalActivityAction", "Individual", "base_CallBehaviorAction" },
                            //new string[] { "IndividualPersonRole", "ActualPerson", "Individual", "base_InstanceSpecification" },
                            new string[] { "IndividualPersonRole", "IndividualPersonRole", "IndividualType", "base_InstanceSpecification" },
                            new string[] { "OrganizationType", "OrganizationType", "IndividualType", "base_Class" },
                            new string[] { "Resource", "Materiel", "IndividualType", "base_Class" },
                            };

        static string[][] MD_Relationship_Lookup = new string[][] {
                            // DM2 Class, UPDM_Profile Element
                            new string[] { "activityPartOfCapability", "ActivityPartOfCapability", "WholePartType","1" },
                            new string[] { "activityPerformedByPerformer", "ActivityPerformedByPerformer", "CoupleType","2" },
                            //new string[] { "OverlapType", "ArbitraryConnector", "CoupleType","1" },
                            new string[] { "OverlapType", "Details", "CoupleType","1" },
                            new string[] { "OverlapType", "Implements", "CoupleType","1" },
                            };

        static string[][] MD_View_Lookup = new string[][] { 
                            new string[] {"CV-2", "CV-2 Capability Taxonomy"},
                            new string[] {"DIV-1", "DIV-1 Conceptual Data Model"},
                            new string[] {"DIV-2", "DIV-2 Logical Data Model"},
                            new string[] {"DIV-3", "DIV-3 Physical Data Model"},
                            new string[] {"OV-1", "OV-1 High-Level Operational Concept Graphic"},
                            new string[] {"OV-2", "OV-2 Operational Resource Flow Description"},
                            new string[] {"OV-4", "OV-4 Organizational Relationships Chart"},
                            new string[] {"OV-5a", "OV-5a Operational Activity Decomposition Tree"},
                            new string[] {"OV-5b", "OV-5b Operational Activity Model"},
                            new string[] {"OV-6c", "OV-6c Operational Event-Trace Description (BPD)"},
                            new string[] {"OV-6c", "OV-6c Operational Event-Trace Description"}, 
                            new string[] {"SV-7", "DODAF2_SV-7Typical"},
                            new string[] {"SvcV-7", "DODAF2_SvcV-7Typical"},
                            new string[] {"SV-1", "SV-1 Systems Interface Description"},
                            new string[] {"SV-2", "SV-2 Systems Communications Description"},
                            new string[] {"SV-4", "SV-4 Systems Functionality Description"},
                            new string[] {"SvcV-1", "SvcV-1 Services Context Description"},
                            new string[] {"SvcV-2", "SvcV-2 Services Resource Flow Description"},
                            new string[] {"SvcV-4", "SvcV-4 Services Functionality Description"},
                            }; 

        static string[][] View_Lookup = new string[][] {  
                            new string[] {"AV-1", "AV-01 Overview and Summary (DM2)", "289", "default"}, 
                            new string[] {"CV-1", "CV-01 Vision (DM2)", "290", "default"},
                            new string[] {"CV-2", "CV-02 Capability Taxonomy (DM2)", "305", "default"}, //1st
                            new string[] {"CV-4", "CV-04 Capability Dependencies (DM2)", "341", "default"},
                            new string[] {"DIV-2", "DIV-02 Logical Data Model (Entity Relation) (DM2)", "4", "default"},
                            new string[] {"DIV-2", "DIV-02 Logical Data Model (IDEF1X) (DM2)", "23", "default"},
                            new string[] {"DIV-3", "DIV-03 Physical Data Model (DM2)", "26", "default"},
                            new string[] {"OV-1", "OV-01 High Level Operational Concept (DM2)", "280", "default"}, //1st
                            new string[] {"OV-2", "OV-02 Operational Resource Flow (DM2)", "281", "default"},
                            new string[] {"OV-2", "OV-02 Operational Resource Flow Alternative (DM2)", "282", "extra"},
                            new string[] {"OV-4", "OV-04 Organizational Relationships (DM2)", "283", "default"},
                            new string[] {"OV-5a", "OV-05a Operational Activity Decomposition (DM2)", "284", "default"}, //1st
                            new string[] {"OV-5b", "OV-05b Operational Activity Model (DM2)", "285", "default"},
                            new string[] {"OV-6a", "OV-06a Operational Rules Model (DM2)", "286", "default"},
                            new string[] {"OV-6b", "OV-06b State Transition (DM2)", "339", "default"},
                            new string[] {"OV-6b", "OV-06b State Transition Alternative (DM2)", "287", "extra"},
                            new string[] {"OV-6c", "OV-06c Performers Event-Trace (DM2)", "340", "extra"},
                            new string[] {"OV-6c", "OV-06c Activities Event-Trace (DM2)", "288", "default"},
                            new string[] {"PV-1", "PV-01 Project Portfolio Relationships (DM2)", "342", "default"},
                            new string[] {"PV-1", "PV-01 Project Portfolio Relationships At Time (DM2)", "343", "extra"},
                            new string[] {"PV-2", "PV-02 Project Timelines (DM2)", "346", "default"},
                            new string[] {"SV-1", "SV-01 Systems Interface Description Alternative (DM2)", "291", "default"},
                            new string[] {"SV-2", "SV-02 Systems Resource Flow Description Alternative (DM2)", "292", "default"},
                            new string[] {"SV-4", "SV-04 Systems Functionality Description (DM2)", "311", "default"},
                            new string[] {"SV-4", "SV-04 Systems Functionality Decomposition (DM2)", "300", "extra"},
                            new string[] {"SV-4", "SV-04 Systems Functionality Description Alternative (DM2)", "293", "extra"},
                            new string[] {"SV-8", "SV-08 Systems Evolution Description (DM2)", "365", "default"},
                            new string[] {"SV-10b", "SV-10b Systems State Transition Description (DM2)", "344", "default"},
                            new string[] {"SV-10c", "SV-10c Performer-Role Event-Trace (DM2x)", "134", "default"},
                            new string[] {"SvcV-1", "SvcV-01 Services Context Description Alternative (DM2)", "301", "default"},
                            new string[] {"SvcV-2", "SvcV-02 Services Resource Flow Description Alternative (DM2)", "302", "default"},
                            new string[] {"SvcV-4", "SvcV-04 Services Functionality Description (DM2)", "314", "default"},
                            new string[] {"SvcV-10b", "SvcV-10b Services State Transition Description (DM2)", "345", "default"},
                            new string[] {"SvcV-10c", "SvcV-10c Performer-Role Event-Trace (DM2x)", "138", "default"},
                            };

        static string[][] Not_Processed_View_Lookup = new string[][] {  
                            new string[] {"SV-1", "SV-01 Systems Interface Description (DM2)", "309", "default"}, 
                            new string[] {"SV-10c", "SV-10c Systems Event-Trace (DM2)", "335", "default"},
                            };

        static string[][] Mandatory_Lookup = new string[][] { 
                            new string[] {"System", "SV-7"},
                            new string[] {"Service", "SvcV-7"},
                            new string[] {"Capability", "CV-2"},
                            new string[] {"ArchitecturalDescription", "OV-1"},
                            new string[] {"Activity", "OV-5a"},
                            new string[] {"Activity", "OV-2"},
                            new string[] {"activityPerformedByPerformer", "OV-2"},
                            new string[] {"activityProducesResource", "OV-2"},
                            new string[] {"activityConsumesResource", "OV-2"},
                            new string[] {"Activity", "OV-3"},
                            new string[] {"activityPerformedByPerformer", "OV-3"},
                            new string[] {"activityProducesResource", "OV-3"},
                            new string[] {"activityConsumesResource", "OV-3"},
                            new string[] {"Activity", "SV-6"},
                            new string[] {"activityPerformedByPerformer", "SV-6"},
                            new string[] {"activityProducesResource", "SV-6"},
                            new string[] {"activityConsumesResource", "SV-6"},
                            new string[] {"System", "SV-6"},
                            new string[] {"Data", "SV-6"},

                            new string[] {"Activity", "SvcV-6"},
                            new string[] {"activityPerformedByPerformer", "SvcV-6"},
                            new string[] {"activityProducesResource", "SvcV-6"},
                            new string[] {"activityConsumesResource", "SvcV-6"},
                            new string[] {"Service", "SvcV-6"},
                            new string[] {"Data", "SvcV-6"},

                            new string[] {"Activity", "SV-5a"},
                            new string[] {"activityPerformedByPerformer", "SV-5a"},
                            new string[] {"System", "SV-5a"},

                            new string[] {"Activity", "Std-2"},
                            new string[] {"activityPerformedByPerformer", "Std-2"},
                            new string[] {"ruleConstrainsActivity", "Std-2"},

                            new string[] {"Activity", "SvcV-5"},
                            new string[] {"activityPerformedByPerformer", "SvcV-5"},
                            new string[] {"Service", "SvcV-5"},

                            new string[] {"Activity", "OV-5b"},
                            new string[] {"activityProducesResource", "OV-5b"},
                            new string[] {"activityConsumesResource", "OV-5b"},
                            new string[] {"Activity", "OV-6b"},
                            new string[] {"activityProducesResource", "OV-6b"},
                            new string[] {"activityConsumesResource", "OV-6b"},
                            new string[] {"Activity", "OV-6a"},
                            new string[] {"Activity", "AV-1"},
                            new string[] {"activityPartOfProjectType", "AV-1"},
                            new string[] {"ArchitecturalDescription", "AV-1"},
                            new string[] {"ProjectType", "AV-1"},
                            new string[] {"Capability", "CV-1"},
                            new string[] {"desiredResourceStateOfCapability", "CV-1"},
                            new string[] {"desireMeasure", "CV-1"},
                            new string[] {"effectMeasure", "CV-1"},
                            new string[] {"MeasureOfDesire", "CV-1"},
                            new string[] {"MeasureOfEffect", "CV-1"},
                            new string[] {"visionRealizedByDesiredResourceState", "CV-1"},
                            new string[] {"Vision", "CV-1"},
                            new string[] {"Capability", "CV-4"},
                            new string[] {"desiredResourceStateOfCapability", "CV-4"},

                            new string[] {"Capability", "CV-6"},
                            new string[] {"Activity", "CV-6"},
                            new string[] {"activityPartOfCapability", "CV-6"},

                            new string[] {"Capability", "CV-3"},
                            new string[] {"Activity", "CV-3"},
                            new string[] {"activityPartOfCapability", "CV-3"},
                            new string[] {"ProjectType", "CV-3"},
                            new string[] {"desiredResourceStateOfCapability", "CV-3"},
                            new string[] {"activityPartOfProjectType", "CV-3"},


                            new string[] {"Activity", "OV-6c"},
                            new string[] {"Activity", "SV-1"},
                            new string[] {"activityPerformedByPerformer", "SV-1"},
                            new string[] {"activityProducesResource", "SV-1"},
                            new string[] {"activityConsumesResource", "SV-1"},
                            new string[] {"System", "SV-1"},
                            new string[] {"Activity", "SV-10b"},
                            new string[] {"activityPerformedByPerformer", "SV-10b"},
                            new string[] {"activityProducesResource", "SV-10b"},
                            new string[] {"activityConsumesResource", "SV-10b"},
                            new string[] {"System", "SV-10b"},
                            new string[] {"Activity", "SV-10c"},
                            new string[] {"activityPerformedByPerformer", "SV-10c"},
                            new string[] {"activityProducesResource", "SV-10c"},
                            new string[] {"activityConsumesResource", "SV-10c"},
                            new string[] {"System", "SV-10c"},
                            new string[] {"Activity", "SvcV-1"},
                            new string[] {"activityPerformedByPerformer", "SvcV-1"},
                            new string[] {"activityProducesResource", "SvcV-1"},
                            new string[] {"activityConsumesResource", "SvcV-1"},
                            new string[] {"Service", "SvcV-1"},
                            new string[] {"Activity", "SvcV-10b"},
                            new string[] {"activityPerformedByPerformer", "SvcV-10b"},
                            new string[] {"activityProducesResource", "SvcV-10b"},
                            new string[] {"activityConsumesResource", "SvcV-10b"},
                            new string[] {"Service", "SvcV-10b"},
                            new string[] {"Activity", "SvcV-10c"},
                            new string[] {"activityPerformedByPerformer", "SvcV-10c"},
                            new string[] {"activityProducesResource", "SvcV-10c"},
                            new string[] {"activityConsumesResource", "SvcV-10c"},
                            new string[] {"Service", "SvcV-10c"},
                            new string[] {"Activity", "SV-4"},
                            new string[] {"activityPerformedByPerformer", "SV-4"},
                            new string[] {"activityProducesResource", "SV-4"},
                            new string[] {"activityConsumesResource", "SV-4"},
                            new string[] {"Data", "SV-4"},
                            new string[] {"System", "SV-4"},
                            new string[] {"Activity", "SvcV-4"},
                            new string[] {"activityPerformedByPerformer", "SvcV-4"},
                            new string[] {"activityProducesResource", "SvcV-4"},
                            new string[] {"activityConsumesResource", "SvcV-4"},
                            new string[] {"Data", "SvcV-4"},
                            new string[] {"Service", "SvcV-4"},
                            new string[] {"Activity", "SV-2"},
                            new string[] {"activityPerformedByPerformer", "SV-2"},
                            new string[] {"activityProducesResource", "SV-2"},
                            new string[] {"activityConsumesResource", "SV-2"},
                            new string[] {"System", "SV-2"},
                            new string[] {"System", "SV-8"},
                            new string[] {"Activity", "SvcV-2"},
                            new string[] {"activityPerformedByPerformer", "SvcV-2"},
                            new string[] {"activityProducesResource", "SvcV-2"},
                            new string[] {"activityConsumesResource", "SvcV-2"},
                            new string[] {"Service", "SvcV-2"},
                            new string[] {"Activity", "PV-1"},
                            new string[] {"activityPartOfProjectType", "PV-1"},
                            new string[] {"ProjectType", "PV-1"},
                            new string[] {"activityPerformedByPerformer", "PV-1"},
                            new string[] {"OrganizationType", "PV-1"},
                            new string[] {"Activity", "PV-2"},
                            new string[] {"activityPartOfProjectType", "PV-2"},
                            new string[] {"ProjectType", "PV-2"},
                            new string[] {"Project", "PV-2"},
                            new string[] {"Data", "DIV-2"},
                            new string[] {"Data", "DIV-3"},
                            new string[] {"DataType", "DIV-3"},
                            };

        static string[][] Optional_Lookup = new string[][] { 
                            new string[] {"PersonRole", "Std-2"},
                            new string[] {"OrganizationType", "Std-2"},
                            new string[] {"Performer", "Std-2"},
                            new string[] {"System", "Std-2"},

                            new string[] {"Measure", "SvcV-7"},
                            new string[] {"MeasureType", "SvcV-7"},
                            new string[] {"measureOfTypeResource", "SvcV-7"},
                            new string[] {"typeInstance", "SvcV-7"},
                            new string[] {"Measure", "SV-7"},
                            new string[] {"MeasureType", "SV-7"},
                            new string[] {"measureOfTypeResource", "SV-7"},
                            new string[] {"typeInstance", "SV-7"},
                            new string[] {"Activity", "CV-1"},
                            new string[] {"Condition", "CV-1"},
                            new string[] {"DomainInformation", "CV-1"},
                            new string[] {"Information", "CV-1"},
                            new string[] {"Location", "CV-1"},
                            new string[] {"Performer", "CV-1"},
                            new string[] {"PersonRole", "CV-1"},
                            new string[] {"Resource", "CV-1"},
                            new string[] {"Rule", "CV-1"}, 
                            new string[] {"System", "CV-1"},
                            new string[] {"Service", "CV-1"},
                            new string[] {"ServiceDescription", "CV-1"},
                            new string[] {"superSubtype", "CV-1"}, 
                            new string[] {"WholePartType", "CV-1"},
                            new string[] {"activityPartOfCapability", "CV-1"}, 
                            new string[] {"activityPartOfCapability", "CV-2"}, 
                            new string[] {"Activity", "CV-2"},
                            new string[] {"Condition", "CV-2"},
                            new string[] {"DomainInformation", "CV-2"},
                            new string[] {"Information", "CV-2"},
                            new string[] {"Location", "CV-2"},
                            new string[] {"Performer", "CV-2"},
                            new string[] {"PersonRole", "CV-2"},
                            new string[] {"Resource", "CV-2"},
                            new string[] {"Rule", "CV-2"}, 
                            new string[] {"System", "CV-2"},
                            new string[] {"Service", "CV-2"},
                            new string[] {"superSubtype", "CV-2"}, 
                            new string[] {"WholePartType", "CV-2"}, 
                            new string[] {"BeforeAfterType", "CV-2"},
                            new string[] {"Activity", "CV-4"},
                            new string[] {"activityPerformedByPerformer", "CV-4"},
                            new string[] {"activityProducesResource", "CV-4"},
                            new string[] {"activityConsumesResource", "CV-4"},
                            new string[] {"BeforeAfterType", "CV-4"},
                            new string[] {"Condition", "CV-4"},
                            new string[] {"DomainInformation", "CV-4"},
                            new string[] {"Information", "CV-4"},
                            new string[] {"Location", "CV-4"},
                            new string[] {"Performer", "CV-4"},
                            new string[] {"PersonRole", "CV-4"},
                            new string[] {"OrganizationType", "CV-4"},
                            new string[] {"Resource", "CV-4"},
                            new string[] {"Rule", "CV-4"}, 
                            new string[] {"System", "CV-4"},
                            new string[] {"Service", "CV-4"},
                            new string[] {"ServiceDescription", "CV-4"},
                            new string[] {"superSubtype", "CV-4"}, 
                            new string[] {"WholePartType", "CV-4"},
                            new string[] {"activityPartOfCapability", "CV-4"}, 

                            new string[] {"activityPerformedByPerformer", "CV-6"},
                            new string[] {"activityProducesResource", "CV-6"},
                            new string[] {"activityConsumesResource", "CV-6"},
                            new string[] {"BeforeAfterType", "CV-6"},
                            new string[] {"Condition", "CV-6"},
                            new string[] {"DomainInformation", "CV-6"},
                            new string[] {"Information", "CV-6"},
                            new string[] {"Location", "CV-6"},
                            new string[] {"Performer", "CV-6"},
                            new string[] {"PersonRole", "CV-6"},
                            new string[] {"OrganizationType", "CV-6"},
                            new string[] {"Resource", "CV-6"},
                            new string[] {"Rule", "CV-6"}, 
                            new string[] {"System", "CV-6"},
                            new string[] {"Service", "CV-6"},
                            new string[] {"ServiceDescription", "CV-6"},
                            new string[] {"superSubtype", "CV-6"}, 
                            new string[] {"WholePartType", "CV-6"},

                            new string[] {"activityPerformedByPerformer", "CV-3"},
                            new string[] {"activityProducesResource", "CV-3"},
                            new string[] {"activityConsumesResource", "CV-3"},
                            new string[] {"BeforeAfterType", "CV-3"},
                            new string[] {"Condition", "CV-3"},
                            new string[] {"DomainInformation", "CV-3"},
                            new string[] {"Information", "CV-3"},
                            new string[] {"Location", "CV-3"},
                            new string[] {"Performer", "CV-3"},
                            new string[] {"PersonRole", "CV-3"},
                            new string[] {"OrganizationType", "CV-3"},
                            new string[] {"Resource", "CV-3"},
                            new string[] {"Rule", "CV-3"}, 
                            new string[] {"System", "CV-3"},
                            new string[] {"Service", "CV-3"},
                            new string[] {"ServiceDescription", "CV-3"},
                            new string[] {"superSubtype", "CV-3"}, 
                            new string[] {"WholePartType", "CV-3"},
                            new string[] {"HappensInType", "CV-3"},
                            new string[] {"PeriodType", "CV-3"},
                            new string[] {"Project", "CV-3"},

                            new string[] {"Information", "OV-1"},
                            new string[] {"Location", "OV-1"},
                            new string[] {"Performer", "OV-1"},
                            new string[] {"Resource", "OV-1"},
                            new string[] {"Rule", "OV-1"}, 
                            new string[] {"superSubtype", "OV-1"}, 
                            new string[] {"WholePartType", "OV-1"},
                            new string[] {"OverlapType", "OV-1"},
                            new string[] {"representationSchemeInstance", "OV-1"},
                            new string[] {"Condition", "OV-2"},
                            new string[] {"Information", "OV-2"},
                            new string[] {"Location", "OV-2"},
                            new string[] {"OrganizationType", "OV-2"},
                            new string[] {"Performer", "OV-2"},
                            new string[] {"PersonRole", "OV-2"},
                            new string[] {"Resource", "OV-2"},
                            new string[] {"Rule", "OV-2"}, 
                            new string[] {"superSubtype", "OV-2"}, 
                            new string[] {"WholePartType", "OV-2"},
                            new string[] {"OverlapType", "OV-2"},
                            new string[] {"Condition", "OV-3"},
                            new string[] {"Information", "OV-3"},
                            new string[] {"Location", "OV-3"},
                            new string[] {"OrganizationType", "OV-3"},
                            new string[] {"Performer", "OV-3"},
                            new string[] {"PersonRole", "OV-3"},
                            new string[] {"Resource", "OV-3"},
                            new string[] {"Rule", "OV-3"}, 
                            new string[] {"superSubtype", "OV-3"}, 
                            new string[] {"WholePartType", "OV-3"},
                            //new string[] {"Condition", "SV-6"},
                            //new string[] {"Information", "SV-6"},
                            //new string[] {"Location", "SV-6"},
                            //new string[] {"OrganizationType", "SV-6"},
                            //new string[] {"Performer", "SV-6"},
                            //new string[] {"PersonRole", "SV-6"},
                            //new string[] {"Resource", "SV-6"},
                            //new string[] {"Rule", "SV-6"}, 
                            //new string[] {"superSubtype", "SV-6"}, 
                            //new string[] {"WholePartType", "SV-6"},
                            new string[] {"Information", "OV-4"},
                            new string[] {"Location", "OV-4"},
                            new string[] {"OrganizationType", "OV-4"},
                            new string[] {"Organization", "OV-4"},
                            new string[] {"Performer", "OV-4"},
                            new string[] {"IndividualPersonRole", "OV-4"},
                            new string[] {"PersonRole", "OV-4"},
                            new string[] {"Resource", "OV-4"},
                            new string[] {"Rule", "OV-4"}, 
                            new string[] {"superSubtype", "OV-4"}, 
                            new string[] {"WholePartType", "OV-4"},
                            new string[] {"OverlapType", "OV-4"},
                            new string[] {"Condition", "OV-5a"},
                            new string[] {"Information", "OV-5a"},
                            new string[] {"Location", "OV-5a"},
                            new string[] {"Performer", "OV-5a"},
                            new string[] {"Resource", "OV-5a"},
                            new string[] {"Rule", "OV-5a"}, 
                            new string[] {"superSubtype", "OV-5a"}, 
                            new string[] {"WholePartType", "OV-5a"}, 
                            new string[] {"Condition", "OV-5b"},
                            new string[] {"Information", "OV-5b"},
                            new string[] {"Location", "OV-5b"},
                            new string[] {"OrganizationType", "OV-5b"},
                            new string[] {"Performer", "OV-5b"},
                            new string[] {"PersonRole", "OV-5b"},
                            new string[] {"Resource", "OV-5b"},
                            new string[] {"Rule", "OV-5b"}, 
                            new string[] {"superSubtype", "OV-5b"}, 
                            new string[] {"WholePartType", "OV-5b"},
                            new string[] {"activityPerformedByPerformer", "OV-5b"},
                            new string[] {"Condition", "OV-6b"},
                            new string[] {"Information", "OV-6b"},
                            new string[] {"Location", "OV-6b"},
                            new string[] {"OrganizationType", "OV-6b"},
                            new string[] {"Performer", "OV-6b"},
                            new string[] {"PersonRole", "OV-6b"},
                            new string[] {"Resource", "OV-6b"},
                            new string[] {"Rule", "OV-6b"}, 
                            new string[] {"superSubtype", "OV-6b"}, 
                            new string[] {"WholePartType", "OV-6b"},
                            new string[] {"activityPerformedByPerformer", "OV-6b"},
                            new string[] {"BeforeAfterType", "OV-6b"},
                            new string[] {"BeforeAfterType", "OV-6c"},
                            new string[] {"Condition", "OV-6c"},
                            new string[] {"Information", "OV-6c"},
                            new string[] {"Location", "OV-6c"},
                            new string[] {"OrganizationType", "OV-6c"},
                            new string[] {"Performer", "OV-6c"},
                            new string[] {"PersonRole", "OV-6c"},
                            new string[] {"Resource", "OV-6c"},
                            new string[] {"Rule", "OV-6c"}, 
                            new string[] {"superSubtype", "OV-6c"}, 
                            new string[] {"WholePartType", "OV-6c"},
                            new string[] {"activityPerformedByPerformer", "OV-6c"},
                            new string[] {"Condition", "OV-6a"},
                            new string[] {"Information", "OV-6a"},
                            new string[] {"Location", "OV-6a"},
                            new string[] {"OrganizationType", "OV-6a"},
                            new string[] {"Performer", "OV-6a"},
                            new string[] {"PersonRole", "OV-6a"},
                            new string[] {"Resource", "OV-6a"},
                            new string[] {"Rule", "OV-6a"}, 
                            new string[] {"superSubtype", "OV-6a"}, 
                            new string[] {"WholePartType", "OV-6a"},
                            new string[] {"ruleConstrainsActivity", "OV-6a"},
                            new string[] {"activityPerformedByPerformer", "OV-6a"},
                            new string[] {"Condition", "AV-1"},
                            new string[] {"Facility", "AV-1"},
                            new string[] {"Guidance", "AV-1"},
                            new string[] {"Information", "AV-1"},
                            new string[] {"Location", "AV-1"},
                            new string[] {"OrganizationType", "AV-1"},
                            new string[] {"Performer", "AV-1"},
                            new string[] {"RealProperty", "AV-1"}, 
                            new string[] {"Resource", "AV-1"},
                            new string[] {"Rule", "AV-1"}, 
                            new string[] {"Site", "AV-1"}, 
                            new string[] {"Vision", "AV-1"},
                            new string[] {"superSubtype", "AV-1"}, 
                            new string[] {"WholePartType", "AV-1"}, 
                            new string[] {"ruleConstrainsActivity", "AV-1"}, 
                            new string[] {"Condition", "SV-1"},
                            new string[] {"Information", "SV-1"},
                            new string[] {"Location", "SV-1"},
                            new string[] {"OrganizationType", "SV-1"},
                            new string[] {"Performer", "SV-1"},
                            new string[] {"PersonRole", "SV-1"},
                            new string[] {"Resource", "SV-1"},
                            new string[] {"Rule", "SV-1"}, 
                            new string[] {"superSubtype", "SV-1"}, 
                            new string[] {"WholePartType", "SV-1"},
                            new string[] {"Condition", "SV-10b"},
                            new string[] {"Information", "SV-10b"},
                            new string[] {"Location", "SV-10b"},
                            new string[] {"OrganizationType", "SV-10b"},
                            new string[] {"Performer", "SV-10b"},
                            new string[] {"PersonRole", "SV-10b"},
                            new string[] {"Resource", "SV-10b"},
                            new string[] {"Rule", "SV-10b"}, 
                            new string[] {"superSubtype", "SV-10b"}, 
                            new string[] {"WholePartType", "SV-10b"},
                            new string[] {"BeforeAfterType", "SV-10b"},
                            new string[] {"Condition", "SV-10c"},
                            new string[] {"Information", "SV-10c"},
                            new string[] {"Location", "SV-10c"},
                            new string[] {"OrganizationType", "SV-10c"},
                            new string[] {"Performer", "SV-10c"},
                            new string[] {"PersonRole", "SV-10c"},
                            new string[] {"Resource", "SV-10c"},
                            new string[] {"Rule", "SV-10c"}, 
                            new string[] {"superSubtype", "SV-10c"}, 
                            new string[] {"WholePartType", "SV-10c"},
                            new string[] {"Condition", "SV-2"},
                            new string[] {"Information", "SV-2"},
                            new string[] {"Location", "SV-2"},
                            new string[] {"OrganizationType", "SV-2"},
                            new string[] {"Performer", "SV-2"},
                            new string[] {"PersonRole", "SV-2"},
                            new string[] {"Resource", "SV-2"},
                            new string[] {"Rule", "SV-2"}, 
                            new string[] {"superSubtype", "SV-2"}, 
                            new string[] {"WholePartType", "SV-2"},
                            new string[] {"Condition", "SvcV-1"},
                            new string[] {"Information", "SvcV-1"},
                            new string[] {"Location", "SvcV-1"},
                            new string[] {"OrganizationType", "SvcV-1"},
                            new string[] {"Performer", "SvcV-1"},
                            new string[] {"PersonRole", "SvcV-1"},
                            new string[] {"Resource", "SvcV-1"},
                            new string[] {"Rule", "SvcV-1"}, 
                            new string[] {"System", "SvcV-1"}, 
                            new string[] {"superSubtype", "SvcV-1"}, 
                            new string[] {"WholePartType", "SvcV-1"},
                            new string[] {"Condition", "SvcV-10b"},
                            new string[] {"Information", "SvcV-10b"},
                            new string[] {"Location", "SvcV-10b"},
                            new string[] {"OrganizationType", "SvcV-10b"},
                            new string[] {"Performer", "SvcV-10b"},
                            new string[] {"PersonRole", "SvcV-10b"},
                            new string[] {"Resource", "SvcV-10b"},
                            new string[] {"Rule", "SvcV-10b"}, 
                            new string[] {"System", "SvcV-10b"}, 
                            new string[] {"superSubtype", "SvcV-10b"}, 
                            new string[] {"WholePartType", "SvcV-10b"},
                            new string[] {"BeforeAfterType", "SvcV-10b"},
                            new string[] {"Condition", "SvcV-10c"},
                            new string[] {"Information", "SvcV-10c"},
                            new string[] {"Location", "SvcV-10c"},
                            new string[] {"OrganizationType", "SvcV-10c"},
                            new string[] {"Performer", "SvcV-10c"},
                            new string[] {"PersonRole", "SvcV-10c"},
                            new string[] {"Resource", "SvcV-10c"},
                            new string[] {"Rule", "SvcV-10c"}, 
                            new string[] {"System", "SvcV-10c"}, 
                            new string[] {"superSubtype", "SvcV-10c"}, 
                            new string[] {"WholePartType", "SvcV-10c"},
                            new string[] {"Condition", "SV-4"},
                            new string[] {"Information", "SV-4"},
                            new string[] {"Location", "SV-4"},
                            new string[] {"OrganizationType", "SV-4"},
                            new string[] {"Performer", "SV-4"},
                            new string[] {"PersonRole", "SV-4"},
                            new string[] {"Resource", "SV-4"},
                            new string[] {"Rule", "SV-4"}, 
                            new string[] {"superSubtype", "SV-4"}, 
                            new string[] {"WholePartType", "SV-4"},
                            new string[] {"Activity", "SV-8"},
                            new string[] {"Condition", "SV-8"},
                            new string[] {"Information", "SV-8"},
                            new string[] {"Location", "SV-8"},
                            new string[] {"OrganizationType", "SV-8"},
                            new string[] {"Performer", "SV-8"},
                            new string[] {"PersonRole", "SV-8"},
                            new string[] {"Resource", "SV-8"},
                            new string[] {"Rule", "SV-8"}, 
                            new string[] {"superSubtype", "SV-8"}, 
                            new string[] {"WholePartType", "SV-8"},
                            new string[] {"BeforeAfterType", "SV-8"},
                            new string[] {"activityPerformedByPerformer", "SV-8"},
                            new string[] {"HappensInType", "SV-8"},
                            new string[] {"PeriodType", "SV-8"},
                            new string[] {"Condition", "SvcV-4"},
                            new string[] {"Information", "SvcV-4"},
                            new string[] {"Location", "SvcV-4"},
                            new string[] {"OrganizationType", "SvcV-4"},
                            new string[] {"Performer", "SvcV-4"},
                            new string[] {"PersonRole", "SvcV-4"},
                            new string[] {"Resource", "SvcV-4"},
                            new string[] {"Rule", "SvcV-4"}, 
                            new string[] {"System", "SvcV-4"},  
                            new string[] {"superSubtype", "SvcV-4"}, 
                            new string[] {"WholePartType", "SvcV-4"},
                            new string[] {"Condition", "SvcV-2"},
                            new string[] {"Information", "SvcV-2"},
                            new string[] {"Location", "SvcV-2"},
                            new string[] {"OrganizationType", "SvcV-2"},
                            new string[] {"Performer", "SvcV-2"},
                            new string[] {"PersonRole", "SvcV-2"},
                            new string[] {"Resource", "SvcV-2"},
                            new string[] {"Rule", "SvcV-2"}, 
                            new string[] {"System", "SvcV-2"}, 
                            new string[] {"superSubtype", "SvcV-2"}, 
                            new string[] {"WholePartType", "SvcV-2"},
                            new string[] {"Condition", "DIV-2"},
                            new string[] {"Information", "DIV-2"},
                            new string[] {"Location", "DIV-2"},
                            new string[] {"OrganizationType", "DIV-2"},
                            new string[] {"Performer", "DIV-2"},
                            new string[] {"Resource", "DIV-2"},
                            new string[] {"Rule", "DIV-2"}, 
                            new string[] {"superSubtype", "DIV-2"}, 
                            new string[] {"WholePartType", "DIV-2"},
                            new string[] {"OverlapType", "DIV-2"},
                            new string[] {"Condition", "DIV-3"},
                            new string[] {"Information", "DIV-3"},
                            new string[] {"Location", "DIV-3"},
                            new string[] {"OrganizationType", "DIV-3"},
                            new string[] {"Performer", "DIV-3"},
                            new string[] {"Resource", "DIV-3"},
                            new string[] {"Rule", "DIV-3"}, 
                            new string[] {"superSubtype", "DIV-3"}, 
                            new string[] {"WholePartType", "DIV-3"},
                            new string[] {"typeInstance", "DIV-3"},
                            new string[] {"OverlapType", "DIV-3"},
                            new string[] {"Condition", "PV-1"},
                            new string[] {"Information", "PV-1"},
                            new string[] {"Location", "PV-1"},
                            new string[] {"Performer", "PV-1"},
                            new string[] {"Resource", "PV-1"},
                            new string[] {"Rule", "PV-1"}, 
                            new string[] {"superSubtype", "PV-1"}, 
                            new string[] {"WholePartType", "PV-1"}, 
                            new string[] {"typeInstance", "PV-2"},
                            new string[] {"PeriodType", "PV-2"},
                            new string[] {"HappensInType", "PV-2"},
                            new string[] {"Condition", "PV-2"},
                            new string[] {"Information", "PV-2"},
                            new string[] {"Location", "PV-2"},
                            new string[] {"Performer", "PV-2"},
                            new string[] {"Resource", "PV-2"},
                            new string[] {"Rule", "PV-2"}, 
                            new string[] {"superSubtype", "PV-2"}, 
                            new string[] {"WholePartType", "PV-2"}, 
                            new string[] {"activityPerformedByPerformer", "PV-2"},
                            new string[] {"Activity", "AV-2"},
                            new string[] {"ArchitecturalDescription", "AV-2"},
                            new string[] {"Capability", "AV-2"},
                            new string[] {"Condition", "AV-2"},
                            new string[] {"Data", "AV-2"},
                            new string[] {"Facility", "AV-2"},
                            new string[] {"Guidance", "AV-2"},
                            new string[] {"Information", "AV-2"},
                            new string[] {"Location", "AV-2"},
                            new string[] {"MeasureOfDesire", "AV-2"},
                            new string[] {"MeasureOfEffect", "AV-2"},
                            new string[] {"OrganizationType", "AV-2"},
                            new string[] {"Performer", "AV-2"},
                            new string[] {"PersonRole", "AV-2"},
                            new string[] {"ProjectType", "AV-2"},
                            new string[] {"RealProperty", "AV-2"}, 
                            new string[] {"Resource", "AV-2"},
                            new string[] {"Rule", "AV-2"}, 
                            new string[] {"Service", "AV-2"}, 
                            new string[] {"ServiceDescription", "AV-2"}, 
                            new string[] {"System", "AV-2"}, 
                            new string[] {"Site", "AV-2"}, 
                            new string[] {"Thing", "AV-2"}, 
                            new string[] {"Vision", "AV-2"},
                            new string[] {"superSubtype", "AV-2"}, 
                            new string[] {"WholePartType", "AV-2"}, 
                            new string[] {"ruleConstrainsActivity", "AV-2"}, 
                            new string[] {"Country", "AV-2"}, 
                            new string[] {"RegionOfCountry", "AV-2"}, 
                            new string[] {"PeriodType", "AV-2"}, 
                            new string[] {"DataType", "AV-2"},

                            new string[] {"Condition", "DIV-1"},
                            new string[] {"Information", "DIV-1"},
                            new string[] {"Location", "DIV-1"},
                            new string[] {"Performer", "DIV-1"},
                            new string[] {"Resource", "DIV-1"},
                            new string[] {"Rule", "DIV-1"}, 
                            new string[] {"superSubtype", "DIV-1"}, 
                            new string[] {"WholePartType", "DIV-1"},
                            new string[] {"OverlapType", "DIV-1"},
                            };

        private class Thing
        {
            public string type;
            public string id; 
            public string name;
            public object value; 
            public string place1;
            public string place2;
            public string foundation;
            public string value_type;
        }

        private class Location
        {
            public string id;
            public string element_id; 
            public string top_left_x;
            public string top_left_y;
            public string top_left_z;
            public string bottom_right_x;
            public string bottom_right_y;
            public string bottom_right_z;
        }

        private class View
        {
            public string type;
            public string id;
            public string name;
            public List<Thing> mandatory;
            public List<Thing> optional;
        }

        private static string Resource_Flow_Type(string type, string view, string place1, string place2, Dictionary<string, Thing> things)
        {
            string type1 = things[place1].type;
            string type2 = things[place2].type;

            if (type == "SF" && view.Contains("SV"))
            {
                return "System Data Flow (DM2rx)";
            }

            if (type == "SF" && view.Contains("SvcV"))
            {
                if (type1 == "Service" && type2 == "Service")
                {
                    return "Service Resource Flow (DM2rx)";
                }
                else
                {
                    return "Service Data Flow (DM2rx)";
                }
            }
                
            if (type == "Needline" && view.Contains("SvcV"))
                return "Physical Resource Flow (DM2rx)";

            if (type == "Needline" && view.Contains("SV"))
            {
                if (type1 == "System" && type2 == "System")    
                    return "System Resource Flow (DM2rx)";
                else
                    return "Physical Resource Flow (DM2rx)";
            }
            else
                return "Need Line (DM2rx)";
                
        }

        public static void Decode(string base64String, string outputFileName)
        {
            byte[] binaryData;
            try
            {
                binaryData =
                   System.Convert.FromBase64String(base64String);
            }
            catch (System.ArgumentNullException)
            {
                System.Console.WriteLine("Base 64 string is null.");
                return;
            }
            catch (System.FormatException)
            {
                System.Console.WriteLine("Base 64 string length is not " +
                   "4 or is not an even multiple of 4.");
                return;
            }

            // Write out the decoded data.
            System.IO.FileStream outFile;
            try
            {
                outFile = new System.IO.FileStream(outputFileName,
                                           System.IO.FileMode.Create,
                                           System.IO.FileAccess.Write);
                outFile.Write(binaryData, 0, binaryData.Length);
                outFile.Close();
            }
            catch (System.Exception exp)
            {
                // Error creating stream or writing to it.
                System.Console.WriteLine("{0}", exp.Message);
            }
        }

        public static string Encode(string inputFileName)
        {
            System.IO.FileStream inFile;
            byte[] binaryData;

            try
            {
                inFile = new System.IO.FileStream(inputFileName,
                                          System.IO.FileMode.Open,
                                          System.IO.FileAccess.Read);
                binaryData = new Byte[inFile.Length];
                long bytesRead = inFile.Read(binaryData, 0,
                                     (int)inFile.Length);
                inFile.Close();
            }
            catch (System.Exception exp)
            {
                // Error creating stream or reading from it.
                System.Console.WriteLine("{0}", exp.Message);
                return null;
            }

            // Convert the binary input into Base64 UUEncoded output. 
            string base64String;
            try
            {
                base64String =
                  System.Convert.ToBase64String(binaryData,
                                         0,
                                         binaryData.Length);
            }
            catch (System.ArgumentNullException)
            {
                System.Console.WriteLine("Binary data array is null.");
                return null;
            }

            return base64String;
        }

        public static void MergeDictionaries<OBJ1, OBJ2>(this IDictionary<OBJ1, List<OBJ2>> dict1, IDictionary<OBJ1, List<OBJ2>> dict2)
        {
            foreach (var kvp2 in dict2)
            {
                if (dict1.ContainsKey(kvp2.Key))
                {
                    dict1[kvp2.Key].AddRange(kvp2.Value);
                    continue;
                }
                dict1.Add(kvp2);
            }
        }

        public static void MergeDictionaries<OBJ1, OBJ2>(this IDictionary<OBJ1, OBJ2> dict1, IDictionary<OBJ1, OBJ2> dict2)
        {
            foreach (KeyValuePair<OBJ1, OBJ2> entry in dict2)
            {
                dict1[entry.Key] = entry.Value;   
            }
        }

        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding
            {
                get { return Encoding.UTF8; }
            }
        }

        private static string Find_DM2_Type (string input) {
        
            foreach(string[] current_lookup in Element_Lookup)
            {
                if (input == current_lookup[1])
                    return current_lookup[0];
            }
            return null;
        }

        private static string Find_DM2_Type_RSA(string input)
        {

            foreach (string[] current_lookup in RSA_Element_Lookup)
            {
                if (input == current_lookup[1])
                    return current_lookup[0];
            }
            return null;
        }

        private static string Find_Def_DM2_Type(string input, List<Thing> things)
        {
            foreach (Thing thing in things)
            {
                if (input == thing.id)
                    return thing.type;
            }
            return null;
        }

        private static string Find_DM2_View(string input)
        {

            foreach (string[] current_lookup in View_Lookup)
            {
                if (input == current_lookup[1])
                    return current_lookup[0];
            }
            return null;
        }
        
        private static string Find_View_MD_Minor_Type(string input)
        {

            foreach (string[] current_lookup in View_Lookup)
            {
                if (input == current_lookup[1])
                    return current_lookup[2];
            }
            return null;
        }

        private static string Find_Symbol_Element_MD_Minor_Type(ref string input, string view)
        {

            foreach (string[] first_lookup in Element_Lookup)
            {
                if (input == first_lookup[1])
                {
                    foreach (string[] second_lookup in MD_Element_Lookup)
                    {
                        if (view == second_lookup[0] && input == second_lookup[1])
                        {
                            input = second_lookup[2];
                            return second_lookup[3];
                        }
                    }
                    return first_lookup[3];
                }
                    
            }
            foreach (string[] current_lookup in MD_Element_Lookup)
            {
                if (input == current_lookup[1])
                    return current_lookup[3];
            }
            return null;
        }

        private static string Find_Definition_Element_MD_Minor_Type(string input)
        {

            foreach (string[] current_lookup in Element_Lookup)
            {
                if (input == current_lookup[1])
                    return current_lookup[4];
            }
            foreach (string[] current_lookup in MD_Element_Lookup)
            {
                if (input == current_lookup[1])
                    return current_lookup[4];
            }
            return null;
        }

        private static string Find_MD_Relationship_Type(string rela_type, string thing_type, string place)
        {

            foreach (string[] current_lookup in Tuple_Lookup)
            {
                if (rela_type == current_lookup[0] && thing_type == current_lookup[4] && (place == "1" ? (current_lookup[3] == "1" || current_lookup[3] == "5") : (current_lookup[3] == "2" || current_lookup[3] == "4")))
                        return current_lookup[1];
            }
            foreach (string[] current_lookup in Tuple_Type_Lookup)
            {
                if (rela_type == current_lookup[0] && thing_type == current_lookup[4] && (place == "1" ? (current_lookup[3] == "1" || current_lookup[3] == "5") : (current_lookup[3] == "2" || current_lookup[3] == "4")))
                    return current_lookup[1];
            }

            return null;
        }

        private static bool Allowed_Element(string view, string id, ref Dictionary<string, Thing> dict)
        {
            Thing value;
            if(dict.TryGetValue(id, out value))
                return Allowed_Class(view, value.type);

            return false;
        }

        private static bool Allowed_Needline(string view, List<Thing> values, ref Dictionary<string, Thing> dict)
        {
            foreach (Thing thing in values)
            {
                if (thing.type == "activityPerformedByPerformer")
                    if (Allowed_Element(view, thing.place1, ref dict) == false)
                        return false;
            }
            
            return true;
        }

        private static bool Allowed_Class(string view, string type)
        {
            foreach (string[] current_lookup in Mandatory_Lookup)
            {
                if (current_lookup[1] != view)
                    continue;

                if (type == current_lookup[0])
                    return true;
            }

            foreach (string[] current_lookup in Optional_Lookup)
            {
                if (current_lookup[1] != view)
                    continue;

                if (type == current_lookup[0])
                    return true;
            }

            return false;
        }

        private static bool Proper_View(List<Thing> input, string name, string type, string id, ref List<string> errors)
        {
            bool found = true;
            bool test = true;
            int count = 0;
            
            foreach (string[] current_lookup in Mandatory_Lookup)  
            {
                if (current_lookup[1] != type)
                    continue;

                found = false;
                foreach (Thing thing in input)
                {
                    if (thing.type == current_lookup[0])
                    {
                        found = true;
                        break;
                    }    
                }

                if (found == false)
                {
                    errors.Add("Diagram error," + id + "," + name + "," + type + ",Missing Mandatory Element: " + current_lookup[0] + "\r\n");
                    test = false;
                    test = false;
                    count++;
                }
            }
            return test;
        }

        private static string Find_Mandatory_Optional(string element, string name, string view, string id, ref List<string> errors)
        {

            foreach (string[] current_lookup in Mandatory_Lookup)
            {
                if (element == current_lookup[0] && view == current_lookup[1])
                    return "Mandatory";
            }

            foreach (string[] current_lookup in Optional_Lookup)
            {
                if (element == current_lookup[0] && view == current_lookup[1])
                    return "Optional";
            }

            errors.Add("Diagram error," + id + "," + name + "," + view + ",Element Ignored. Type Not Allowed: " + element + "\r\n");
            return "$none$";
        }

        private static void Add_Tuples(ref List<List<Thing>> input_list, ref List<List<Thing>> sorted_results, List<Thing> relationships, ref List<string> errors) 
        {
            //List<List<Thing>> sorted_results = input_list_new;
            bool place1 = false;
            bool place2 = false;
            Thing value;
            
            //foreach (List<Thing> old_view in input_list)
            for(int i=0;i<input_list.Count;i++)
            {
                List<Thing> things_view = new List<Thing>();
                List<Thing> new_view = new List<Thing>();
                List<Thing> other_view = new List<Thing>();
                Dictionary<string, Thing> dic;


                //if (old_view.Where(x => x.value != null).Where(x => (string)x.value == "$none$").Count() > 0)
                //    new_view = old_view.Where(x => x.value != null).Where(x => (string)x.value == "$none$").ToList();

                other_view = input_list[i].Where(x => x.value != null).Where(x => (string)x.value != "$none$" && ((string)x.value).Substring(0, 1) != "_").ToList();

                if (sorted_results.Count == i)
                {
                    
                    new_view.AddRange(input_list[i].Where(x => x.value == null).ToList());
                    new_view.AddRange(input_list[i].Where(x => x.value != null).Where(x => ((string)x.value).Substring(0, 1) == "_"));


                    foreach (Thing thing in other_view)
                    {
                        if (Find_Mandatory_Optional((string)thing.value, other_view.First().name, thing.type, thing.place1, ref errors) != "$none$")
                            new_view.Add(thing);
                    }

                    //remove
                    //var duplicateKeys = new_view.GroupBy(x => x.place2)
                    //        .Where(group => group.Count() > 1)
                    //        .Select(group => group.Key);

                    //List<string> test = duplicateKeys.ToList();

                    new_view = new_view.GroupBy(x => x.place2).Select(y => y.First()).ToList();

                    dic = new_view.Where(x => x.place2 != null).ToDictionary(x => x.place2, x => x);

                }
                else
                {
                    other_view = other_view.GroupBy(x => x.place2).Select(y => y.First()).ToList();

                    dic = other_view.Where(x => x.place2 != null).ToDictionary(x => x.place2, x => x);
                }
                
                
                foreach (Thing rela in relationships)
                {
                    place1 = false;
                    place2 = false;

                    if (dic.TryGetValue(rela.place1, out value))
                        place1 = true;

                    if (dic.TryGetValue(rela.place2, out value))
                        place2 = true;

                    if (place1 && place2)
                    {
                        new_view.Add(new Thing { place1 = value.place1, place2 = rela.id, value = rela.type, type = value.type, value_type="$none$" });
                    }
                }

                if (sorted_results.Count == i)
                    sorted_results.Add(new_view);
                else
                    sorted_results[i] = sorted_results[i].Union(new_view).ToList();
            }

           // return sorted_results;
        }

        private static List<Thing> Add_Places(Dictionary<string,Thing> things, List<Thing> values)
        {
            values = values.Distinct().ToList();
            IEnumerable<Thing> results = new List<Thing>(values);
            List<Thing> places = new List<Thing>();
            Thing value;

            foreach (Thing rela in values)
            {

                if(things.TryGetValue(rela.place1, out value))
                    places.Add(value);

                if (things.TryGetValue(rela.place2, out value))
                    places.Add(value);

            }

            results = results.Concat(places.Distinct());
            return results.ToList();
        }

        private static List<List<Thing>> Get_Tuples_place1(Thing input, IEnumerable<Thing> relationships)
        {
            List<Thing> results = new List<Thing>();

            foreach (Thing rela in relationships)
            {

                    if (input.id == rela.place1)
                    {
                        results.Add(new Thing { id = rela.id, type = Find_MD_Relationship_Type(rela.type, input.type,"1"), place1 = input.id, place2 = rela.place2, value = input, value_type = "$Thing$" });
                    }                    
            }

            return results.GroupBy(x => x.type).Select(group => group.Distinct().ToList()).ToList(); ;
        }

        private static List<List<Thing>> Get_Tuples_place2(Thing input, IEnumerable<Thing> relationships)
        {
            List<Thing> results = new List<Thing>();

            foreach (Thing rela in relationships)
            {

                if (input.id == rela.place2)
                {
                    results.Add(new Thing { id = rela.id, type = Find_MD_Relationship_Type(rela.type, input.type,"2"), place1 = input.id, place2 = rela.place1, value = input, value_type = "$Thing$" });
                }
            }

            return results.GroupBy(x => x.type).Select(group => group.Distinct().ToList()).ToList(); ;
        }

        private static List<List<Thing>> Get_Tuples_id(Thing input, IEnumerable<Thing> relationships)
        {
            List<Thing> results = new List<Thing>();

            foreach (Thing rela in relationships)
            {

                if (input.id == rela.id)
                {
                    results.Add(new Thing { id = rela.id, type = "performerTarget", place1 = input.id, place2 = rela.place1, value = input, value_type = "$Thing$" });
                    results.Add(new Thing { id = rela.id, type = "performerSource", place1 = input.id, place2 = rela.place2, value = input, value_type = "$Thing$" });
                }
            }

            return results.GroupBy(x => x.type).Select(group => group.Distinct().ToList()).ToList(); ;
        }

        ////////////////////
        ////////////////////

//        public static bool MD2PES_HighLevel(byte[] input, ref string output, ref string errors)
//        {
//            IEnumerable<Thing> things = new List<Thing>();
//            IEnumerable<Thing> tuple_types = new List<Thing>();
//            IEnumerable<Thing> tuples = new List<Thing>();
//            IEnumerable<Thing> results;
//            IEnumerable<Thing> results2;
//            IEnumerable<Location> locations;
//            List<View> views = new List<View>();
//            List<Thing> mandatory_list = new List<Thing>();
//            List<Thing> optional_list = new List<Thing>();
//            string temp;
//            Dictionary<string, List<Thing>> doc_blocks_data;
//            Dictionary<string, string> diagrams;
//            Dictionary<string, string> not_processed_diagrams;
//            Dictionary<string, Thing> things_dic;
//            Dictionary<string, Thing> values_dic;
//            Dictionary<string, Thing> values_dic2;
//            Dictionary<string, List<Thing>> doc_blocks_views = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> description_views = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> OV1_pic_views = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> needline_mandatory_views = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> CV1_mandatory_views = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> CV1_optional_views = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> CV4_mandatory_views = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> CV4_optional_views = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> needline_optional_views = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> OV2_support_mandatory_views = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> OV2_support_optional_views = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> OV4_support_optional_views = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> OV2_support_mandatory_views_2 = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> OV4_support_optional_views_2 = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> OV5b_aro_mandatory_views = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> OV6c_aro_optional_views = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> OV5b_aro_optional_views = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> PV1_mandatory_views = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> DIV3_optional = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> DIV3_mandatory = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> results_dic;
//            Dictionary<string, List<Thing>> period_dic = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> datatype_mandatory_dic = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> datatype_optional_dic = new Dictionary<string, List<Thing>>();
//            Dictionary<string, List<Thing>> aro;
//            Dictionary<string, List<Thing>> aro2;
//            XElement root = XElement.Load(new MemoryStream(input));
//            List<List<Thing>> sorted_results = new  List<List<Thing>>();
//            List<List<Thing>> sorted_results_new = new List<List<Thing>>();
//            List<List<Thing>> view_holder = new List<List<Thing>>();
//            bool representation_scheme = false;
//            List<Thing> values = new List<Thing>();
//            List<Thing> values2 = new List<Thing>();
//            List<Thing> values3 = new List<Thing>();
//            List<Thing> values4 = new List<Thing>();
//            List<Thing> values5 = new List<Thing>();
//            List<Thing> values6 = new List<Thing>();
//            List<Thing> values7 = new List<Thing>();
//            Thing value;
//            Thing value2;
//            int count = 0;
//            int count2 = 0;
//            bool add = false;
//            bool test = true;
//            List<string> errors_list = new List<string>();


//            //Diagram Type

//            results =
//                from result in root.Elements("Class").Elements("MDDiagram")
//                select new Thing
//                            {
//                                type = (string)result.Attribute("MDObjMinorTypeName"),
//                                id = (string)result.Attribute("MDObjId"),
//                                name = ((string)result.Attribute("MDObjName")).Replace("&", " And "),
//                                value = "$none$",
//                                place1 = "$none$",
//                                place2 = "$none$",
//                                foundation = "Thing",
//                                value_type = "$none$"
//                            };

//            diagrams = View_Lookup.ToDictionary(x => x[1], x => x[0]);
//            not_processed_diagrams = Not_Processed_View_Lookup.ToDictionary(x => x[1], x => x[0]);
//            foreach (Thing thing in results)
//            {
//                if (!diagrams.TryGetValue(thing.type, out temp))
//                {
//                    if (not_processed_diagrams.TryGetValue(thing.type, out temp))
//                    {
//                        errors_list.Add("Diagram error," + thing.id + "," + thing.name + "," + temp + ", Type Not Allowed - Diagram Ignored: " + thing.type + "\r\n");
//                    }
//                    else
//                    {
//                        errors_list.Add("Diagram error," + thing.id + "," + thing.name + ",Unknown, Type Not Allowed - Diagram Ignored: " + thing.type + "\r\n");
//                    }
//                }
//            }



//            //Doc Block

//            results_dic =
//                (from result in root.Elements("Class").Elements("MDDiagram").Elements("MDSymbol")
//                where (string)result.Attribute("MDObjMinorTypeName") == "Doc Block"
//                select new
//                {
//                        key = (string)result.Parent.Attribute("MDObjId"),
//                        value = new List<Thing> {new Thing
//                        {
//                        type = "Information",
//                        id = (string)result.Attribute("MDObjId")+"_1",
//                        name = "Doc Block Comment",
//                        value = (string)result.Attribute("MDSymZPDesc"),
//                        place1 = "$none$",
//                        place2 = "$none$",
//                        foundation = "IndividualType",
//                        value_type = "exemplar"
//                        },new Thing
//                    {
//                        type = "Information",
//                        id = (string)result.Attribute("MDObjId")+"_2",
//                        name = "Doc Block Type",
//                        value = (string)result.Parent.Attribute("MDObjMinorTypeName"),
//                        place1 = "$none$",
//                        place2 = "$none$",
//                        foundation = "IndividualType",
//                        value_type = "exemplar"
//                    },new Thing
//                    {
//                        type = "Information",
//                        id = (string)result.Attribute("MDObjId")+"_3",
//                        name = "Doc Block Date",
//                        value = (string)result.Parent.Attribute("MDObjUpdateDate"),
//                        place1 = "$none$",
//                        place2 = "$none$",
//                        foundation = "IndividualType",
//                        value_type = "exemplar"
//                    },new Thing
//                    {
//                        type = "Information",
//                        id = (string)result.Attribute("MDObjId")+"_4",
//                        name = "Doc Block Time",
//                        value = (string)result.Parent.Attribute("MDObjUpdateTime"),
//                        place1 = "$none$",
//                        place2 = "$none$",
//                        foundation = "IndividualType",
//                        value_type = "exemplar"
//                    }}
//                }).ToDictionary(a => a.key, a => a.value);

//            if(results_dic.Count() > 0)
//            {
//                doc_blocks_data = new Dictionary<string, List<Thing>>(results_dic);

//                foreach (KeyValuePair<string, List<Thing>> entry in results_dic)
//                {
//                    List<Thing> thing_list = new List<Thing>();
//                    foreach (Thing thing in entry.Value)
//                    {
//                        thing_list.Add(new Thing
//                        {
//                            type = "describedBy", id = thing.id + entry.Key, foundation = "namedBy", place1 = entry.Key,
//                            place2 = thing.id, name = "$none$", value = "$none$", value_type = "$none$"
//                        });
//                    }
//                    tuples = tuples.Concat(thing_list);

//                    doc_blocks_views.Add(entry.Key,new List<Thing> (thing_list));
//                }

//                results_dic =
//                    (from result in root.Elements("Class").Elements("MDDiagram").Elements("MDSymbol")
//                        where (string)result.Attribute("MDObjMinorTypeName") == "Doc Block"
//                        select new
//                        {
//                            key = (string)result.Parent.Attribute("MDObjId"),
//                            value = new List<Thing> {new Thing
//                            {
//                                type = "Thing",
//                                id = (string)result.Attribute("MDObjId"),
//                                name = ((string)result.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                                value = "$none$",
//                                place1 = "$none$",
//                                place2 = "$none$",
//                                foundation = "Thing",
//                                value_type = "$none$"
//                            }}
//                        }).ToDictionary(a => a.key, a => a.value);

//                MergeDictionaries(doc_blocks_data, results_dic);

//                things = things.Concat(doc_blocks_data.SelectMany(x => x.Value));

//                MergeDictionaries(doc_blocks_views, doc_blocks_data);
//            }

//            //Regular Things

//            foreach(string[] current_lookup in Element_Lookup)
//            {

//                results =
//                    from result in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result.Attribute("MDObjMinorTypeName") == current_lookup[1]
//                    select new Thing
//                    {
//                        type = current_lookup[0],
//                        id = (string)result.Attribute("MDObjId"),
//                        name = ((string)result.Attribute("MDObjName")).Replace("&", " And "),
//                        value = "$none$",
//                        place1 = "$none$",
//                        place2 = "$none$",
//                        foundation = current_lookup[2],
//                        value_type = "$none$"
//                    };

//                things = things.Concat(results.ToList());

//                if (current_lookup[1] != "Entity" && current_lookup[1] != "Access Path" && current_lookup[1] != "Index" && current_lookup[1] != "Table")
//                {
//                    results_dic =
//                        (from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty")
//                         where (string)result.Parent.Attribute("MDObjMinorTypeName") == current_lookup[1]
//                         where (string)result.Attribute("MDPrpName") == "Description"
//                         select new
//                         {
//                             key = (string)result.Parent.Attribute("MDObjId"),
//                             value = new List<Thing> {
//                            new Thing
//                            {
//                                type = "Information",
//                                id = (string)result.Parent.Attribute("MDObjId") + "_9",
//                                name = ((string)result.Parent.Attribute("MDObjName")).Replace("&", " And ") + " Description",
//                                value = ((string)result.Attribute("MDPrpValue")).Replace("@", " At ").Replace("\"","'").Replace("&", " And "),
//                                place1 = (string)result.Parent.Attribute("MDObjId"),
//                                place2 = (string)result.Parent.Attribute("MDObjId") + "_9",
//                                foundation = "IndividualType",
//                                value_type = "exemplar"
//                            }
//                        }
//                         }).ToDictionary(a => a.key, a => a.value);

//                    things = things.Concat(results_dic.SelectMany(x => x.Value));

//                    foreach (Thing thing in results_dic.SelectMany(x => x.Value))
//                    {
//                        value = new Thing
//                        {
//                            type = "describedBy",
//                            id = thing.place1 + "_10",
//                            foundation = "namedBy",
//                            place1 = thing.place1,
//                            place2 = thing.place2,
//                            name = "$none$",
//                            value = "$none$",
//                            value_type = "$none$"
//                        };
//                        tuples = tuples.Concat(new List<Thing> { value });
//                        description_views.Add(thing.place1, new List<Thing> { value });
//                    }

//                    MergeDictionaries(description_views, results_dic);
//                }
//                else if (current_lookup[1] == "Index")
//                {
//                    results =
//                        from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                         where (string)result.Parent.Parent.Attribute("MDObjMinorTypeName") == current_lookup[1]
//                         where (string)result.Parent.Attribute("MDPrpName") == "Description"
                         
//                         select new Thing
//                            {
//                                type = "Information",
//                                id = (string)result.Parent.Parent.Attribute("MDObjId") + (string)result.Attribute("MDLinkIdentity") + "_9",
//                                name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And ") + " Primary Key",
//                                value = (string)result.Attribute("MDLinkIdentity"),
//                                place1 = (string)result.Parent.Parent.Attribute("MDObjId"),
//                                place2 = (string)result.Parent.Parent.Attribute("MDObjId") + (string)result.Attribute("MDLinkIdentity") + "_9",
//                                foundation = "IndividualType",
//                                value_type = "exemplar"
//                            };

//                    things = things.Concat(results);

//                    sorted_results = results.GroupBy(x => x.place1).Select(group => group.ToList()).ToList();

//                    foreach (List<Thing> view in sorted_results)
//                    {
//                        values = new List<Thing>();
//                        foreach (Thing thing in view)
//                        {
//                            value = new Thing
//                            {
//                                type = "describedBy",
//                                id = thing.place2 + "_10",
//                                foundation = "namedBy",
//                                place1 = thing.place1,
//                                place2 = thing.place2,
//                                name = "$none$",
//                                value = "$none$",
//                                value_type = "$none$"
//                            };
//                            tuples = tuples.Concat(new List<Thing> { value });
//                            values.Add(value);
//                            values.Add(thing);
//                        }
//                        description_views.Add(view.First().place1, values);
//                    }

//                    //MergeDictionaries(description_views, results_dic);
//                }
//            }

//            //OV-1 Picture

//            results =
//                from result in root.Elements("Class").Elements("MDDiagram").Elements("MDSymbol").Elements("MDPicture")
//                where (string)result.Parent.Attribute("MDObjMinorTypeName") == "Picture"
//                where (string)result.Parent.Parent.Attribute("MDObjMinorTypeName") == "OV-01 High Level Operational Concept (DM2)"
//                select 
//                //new {
//                //    key = (string)result.Parent.Parent.Attribute("MDObjId"),
//                //    value = new List<Thing> {
//                        new Thing
//                    {
//                    type = "ArchitecturalDescription",
//                    id = (string)result.Parent.Attribute("MDObjId"),
//                    name = ((string)result.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                    value = (string)result.Attribute("MDPictureData"),
//                    place1 = (string)result.Parent.Parent.Attribute("MDObjId"),
//                    place2 = (string)result.Parent.Attribute("MDObjId"),
//                    foundation = "IndividualType",
//                    value_type = "exemplar"
//                    };
//                //}}).ToDictionary(a => a.key, a => a.value);

//            OV1_pic_views = results.GroupBy(x=>x.place1).ToDictionary(x=>x.Key, x=>x.ToList());

//            if (OV1_pic_views.Count() > 0)
//            {
//                representation_scheme = true;
//                foreach (KeyValuePair<string, List<Thing>> entry in OV1_pic_views)
//                {
//                    foreach (Thing thing in entry.Value)
//                    {
//                        tuples = tuples.Concat(new List<Thing>{new Thing
//                            {
//                            type = "representationSchemeInstance",
//                            id = thing.id+"_1",
//                            name = "$none$",
//                            value = "$none$",
//                            place1 = "_rs1",
//                            place2 = thing.id,
//                            foundation = "typeInstance",
//                            value_type = "$none$"
//                            }});
//                    }
//                }
//                things = things.Concat(OV1_pic_views.SelectMany(x => x.Value));
//            }

//            //Regular tuples

//            foreach (string[] current_lookup in Tuple_Lookup)
//            {
//                if (current_lookup[3] == "1")
//                {
//                    results =
//                        from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                        where (string)result.Parent.Attribute("MDPrpName") == current_lookup[1]
//                        select new Thing
//                        {
//                            type = current_lookup[0],
//                            id = (string)result.Parent.Parent.Attribute("MDObjId") + (string)result.Attribute("MDLinkIdentity"),
//                            name = "$none$",
//                            value = "$none$",
//                            place1 = (string)result.Parent.Parent.Attribute("MDObjId"),
//                            place2 = (string)result.Attribute("MDLinkIdentity"),
//                            foundation = current_lookup[2],
//                            value_type = "$none$"
//                        };
//                }
//                else
//                {
//                    results =
//                        from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                        where (string)result.Parent.Attribute("MDPrpName") == current_lookup[1]
//                        select new Thing
//                        {
//                            type = current_lookup[0],
//                            id = (string)result.Attribute("MDLinkIdentity") + (string)result.Parent.Parent.Attribute("MDObjId"),
//                            name = "$none$",
//                            value = "$none$",
//                            place2 = (string)result.Parent.Parent.Attribute("MDObjId"),
//                            place1 = (string)result.Attribute("MDLinkIdentity"),
//                            foundation = current_lookup[2],
//                            value_type = "$none$"
//                        };
//                }
//                tuples = tuples.Concat(results.ToList());
//            }

//            tuples = tuples.GroupBy(x => x.id).Select(grp => grp.First());

//            //Regular TupleTypes

//            foreach (string[] current_lookup in Tuple_Type_Lookup)
//            {
//                if (current_lookup[3] == "1")
//                {
//                    results =
//                        from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                        where (string)result.Parent.Attribute("MDPrpName") == current_lookup[1]
//                        select new Thing
//                        {
//                            type = current_lookup[0],
//                            id = (string)result.Parent.Parent.Attribute("MDObjId") + (string)result.Attribute("MDLinkIdentity"),
//                            name = "$none$",
//                            value = "$none$",
//                            place1 = (string)result.Parent.Parent.Attribute("MDObjId"),
//                            place2 = (string)result.Attribute("MDLinkIdentity"),
//                            foundation = current_lookup[2],
//                            value_type = "$none$"
//                        };

//                    tuple_types = tuple_types.Concat(results.ToList());

//                }
//                else if (current_lookup[3] == "2")
//                {
//                    results =
//                        from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                        where (string)result.Parent.Attribute("MDPrpName") == current_lookup[1]
//                        select new Thing
//                        {
//                            type = current_lookup[0],
//                            id = (string)result.Attribute("MDLinkIdentity") + (string)result.Parent.Parent.Attribute("MDObjId"),
//                            name = "$none$",
//                            value = "$none$",
//                            place2 = (string)result.Parent.Parent.Attribute("MDObjId"),
//                            place1 = (string)result.Attribute("MDLinkIdentity"),
//                            foundation = current_lookup[2],
//                            value_type = "$none$"
//                        };

//                    tuple_types = tuple_types.Concat(results.ToList());

//                }
//                else if (current_lookup[3] == "4")
//                {
//                    results =
//                        from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                        where (string)result.Parent.Attribute("MDPrpName") == current_lookup[1]
//                        where (string)result.Parent.Parent.Attribute("MDObjMinorTypeName") == current_lookup[4]
//                        select new Thing
//                        {
//                            type = current_lookup[0],
//                            id = (string)result.Attribute("MDLinkIdentity") + (string)result.Parent.Parent.Attribute("MDObjId"),
//                            name = "$none$",
//                            value = "$none$",
//                            place2 = (string)result.Parent.Parent.Attribute("MDObjId"),
//                            place1 = (string)result.Attribute("MDLinkIdentity"),
//                            foundation = current_lookup[2],
//                            value_type = "$none$"
//                        };

//                    tuple_types = tuple_types.Concat(results.ToList());

//                }
//                else if (current_lookup[3] == "5")
//                {
//                    results =
//                        from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                        where (string)result.Parent.Attribute("MDPrpName") == current_lookup[1]
//                        where (string)result.Parent.Parent.Attribute("MDObjMinorTypeName") == current_lookup[4]
//                        select new Thing
//                        {
//                            type = current_lookup[0],
//                            id = (string)result.Parent.Parent.Attribute("MDObjId") + (string)result.Attribute("MDLinkIdentity"),
//                            name = "$none$",
//                            value = "$none$",
//                            place1 = (string)result.Parent.Parent.Attribute("MDObjId"),
//                            place2 = (string)result.Attribute("MDLinkIdentity"),
//                            foundation = current_lookup[2],
//                            value_type = "$none$"
//                        };

//                    tuple_types = tuple_types.Concat(results.ToList());

//                }
                
//            }

//            tuple_types = tuple_types.GroupBy(x => x.id).Select(grp => grp.First());

//            //CV-1

//            results =
//                    from result in root.Elements("Class").Elements("MDDiagram").Elements("MDSymbol")
//                    where (string)result.Parent.Attribute("MDObjMinorTypeName") == "CV-01 Vision (DM2)"
//                    where (string)result.Attribute("MDObjMinorTypeName") == "Capability (DM2)"
//                    select new Thing
//                    {
//                        type = "CV-01 View",
//                        id = "$none$",
//                        name = ((string)result.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                        value = ((string)result.Attribute("MDObjName")).Replace("&", " And "),
//                        place1 = (string)result.Parent.Attribute("MDObjId"),
//                        place2 = (string)result.Attribute("MDSymIdDef"),
//                        foundation = "$none$",
//                        value_type = "$Capability Name$"
//                    };

//            if (results.Count() > 0)
//            {
//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "Vision",
//                    id = results.First().place1 + "_1",
//                    name = results.First().name,
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                things = things.Concat(values);

//                CV1_mandatory_views.Add(results.First().place1, values); 
//            }

//            foreach (Thing thing in results.ToList())
//            {
//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "Resource",
//                    id = thing.place2 + "_1",
//                    name = thing.value + "_DesiredResourceState",
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "MeasureOfEffect",
//                    id = thing.place2 + "_4",
//                    name = thing.value + "_MeasureOfEffect",
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "MeasureOfDesire",
//                    id = thing.place2 + "_5",
//                    name = thing.value + "_MeasureOfDesire",
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                things = things.Concat(values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "visionRealizedByDesiredResourceState",
//                    id = thing.place2 + "_2",
//                    name = thing.value + "_visionRealizedByDesiredResourceState",
//                    value = "$none$",
//                    place1 = thing.place1 + "_1",
//                    place2 = thing.place2 + "_1",
//                    foundation = "CoupleType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "desiredResourceStateOfCapability",
//                    id = thing.place2 + "_3",
//                    name = thing.value + "_desiredResourceStateOfCapability",
//                    value = "$none$",
//                    place1 = thing.place2,
//                    place2 = thing.place2 + "_1",
//                    foundation = "WholePartType",
//                    value_type = "$none$"
//                });

//                tuple_types = tuple_types.Concat(values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "effectMeasure",
//                    id = thing.place2 + "_6",
//                    name = thing.value + "_effectMeasure",
//                    value = "$none$",
//                    place1 = thing.place2 + "_1",
//                    place2 = thing.place2 + "_4",
//                    foundation = "superSubtype",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "desireMeasure",
//                    id = thing.place2 + "_7",
//                    name = thing.value + "_desireMeasure",
//                    value = "$none$",
//                    place1 = thing.place2 + "_1",
//                    place2 = thing.place2 + "_5",
//                    foundation = "superSubtype",
//                    value_type = "$none$"
//                });

//                tuples = tuples.Concat(values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "Resource",
//                    id = thing.place2 + "_1",
//                    name = thing.value + "_DesiredResourceState",
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                CV1_optional_views.Add(thing.place2, values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "MeasureOfEffect",
//                    id = thing.place2 + "_4",
//                    name = thing.value + "_MeasureOfEffect",
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "MeasureOfDesire",
//                    id = thing.place2 + "_5",
//                    name = thing.value + "_MeasureOfDesire",
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "visionRealizedByDesiredResourceState",
//                    id = thing.place2 + "_2",
//                    name = thing.value + "_visionRealizedByDesiredResourceState",
//                    value = "$none$",
//                    place1 = thing.place1 + "_1",
//                    place2 = thing.place2 + "_1",
//                    foundation = "CoupleType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "desiredResourceStateOfCapability",
//                    id = thing.place2 + "_3",
//                    name = thing.value + "_desiredResourceStateOfCapability",
//                    value = "$none$",
//                    place1 = thing.place2,
//                    place2 = thing.place2 + "_1",
//                    foundation = "WholePartType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "effectMeasure",
//                    id = thing.place2 + "_6",
//                    name = thing.value + "_effectMeasure",
//                    value = "$none$",
//                    place1 = thing.place2 + "_1",
//                    place2 = thing.place2 + "_4",
//                    foundation = "superSubtype",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "desireMeasure",
//                    id = thing.place2 + "_7",
//                    name = thing.value + "_desireMeasure",
//                    value = "$none$",
//                    place1 = thing.place2 + "_1",
//                    place2 = thing.place2 + "_5",
//                    foundation = "superSubtype",
//                    value_type = "$none$"
//                });

//                CV1_mandatory_views.Add(thing.place2, values);  
//            }

//            //CV-4

//            results =
//                from result in root.Elements("Class").Elements("MDDiagram").Elements("MDSymbol")
//                where (string)result.Parent.Attribute("MDObjMinorTypeName") == "CV-04 Capability Dependencies (DM2)"
//                where (string)result.Attribute("MDObjMinorTypeName") == "Capability (DM2)"
//                select new Thing
//                {
//                    type = "CV-04 View",
//                    id = "$none$",
//                    name = ((string)result.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                    value = ((string)result.Attribute("MDObjName")).Replace("&", " And "),
//                    place1 = (string)result.Parent.Attribute("MDObjId"),
//                    place2 = (string)result.Attribute("MDSymIdDef"),
//                    foundation = "$none$",
//                    value_type = "$Capability Name$"
//                };

//            foreach (Thing thing in results.ToList())
//            {
//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "Resource",
//                    id = thing.place2 + "_1",
//                    name = thing.value + "_DesiredResourceState",
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                things = things.Concat(values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "desiredResourceStateOfCapability",
//                    id = thing.place2 + "_3",
//                    name = thing.value + "_desiredResourceStateOfCapability",
//                    value = "$none$",
//                    place1 = thing.place2,
//                    place2 = thing.place2 + "_1",
//                    foundation = "WholePartType",
//                    value_type = "$none$"
//                });

//                tuple_types = tuple_types.Concat(values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "Resource",
//                    id = thing.place2 + "_1",
//                    name = thing.value + "_DesiredResourceState",
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                values2 = new List<Thing>();
//                if(!CV1_optional_views.TryGetValue(thing.place2, out values2))
//                    CV1_optional_views.Add(thing.place2, values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "desiredResourceStateOfCapability",
//                    id = thing.place2 + "_3",
//                    name = thing.value + "_desiredResourceStateOfCapability",
//                    value = "$none$",
//                    place1 = thing.place2,
//                    place2 = thing.place2 + "_1",
//                    foundation = "WholePartType",
//                    value_type = "$none$"
//                });

//                values2 = new List<Thing>();
//                if (!CV4_mandatory_views.TryGetValue(thing.place2, out values2))
//                    CV4_mandatory_views.Add(thing.place2, values);
//            }

//            // Data Store

//            things = things.GroupBy(x => x.id).Select(grp => grp.First());
//            things_dic = things.ToDictionary(x => x.id, x => x);

//            results =
//                from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                where (string)result.Parent.Attribute("MDPrpName") == "Resources"
//                where (string)result.Parent.Parent.Attribute("MDObjMinorTypeName") == "Data Store (DM2x)"

//                select new Thing
//                {
//                    type = "WholePartType",
//                    id = (string)result.Parent.Parent.Attribute("MDObjId") + (string)result.Attribute("MDLinkIdentity"),
//                    name = "$none$",
//                    value = "$none$",
//                    place1 = (string)result.Parent.Parent.Attribute("MDObjId"),
//                    place2 = (string)result.Attribute("MDLinkIdentity"),
//                    foundation = "WholePartType",
//                    value_type = "$none$"
//                };

//            tuple_types = tuple_types.Concat(results.ToList());

//            foreach (Thing thing in results)
//            {
//                if (!things_dic.TryGetValue(thing.place2, out value))
//                {
//                    values = new List<Thing>();

//                    values.Add(new Thing
//                    {
//                        type = "Data",
//                        id = thing.place2,
//                        name = "$none$",
//                        value = "$none$",
//                        place1 = "$none$",
//                        place2 = "$none$",
//                        foundation = "IndividualType",
//                        value_type = "$none$"
//                    });

//                    things = things.Concat(values);
//                    things_dic.Add(values.First().id, values.First());
//                }
//            }

//            //ARO

//            results =
//                   from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                   where (string)result.Parent.Attribute("MDPrpName") == "consumingActivity"
//                   from result3 in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                   where (string)result3.Parent.Attribute("MDPrpName") == "Resources"
//                   where (string)result.Parent.Parent.Attribute("MDObjId") == (string)result3.Parent.Parent.Attribute("MDObjId")
//                   from result2 in root.Elements("Class").Elements("MDDefinition")
//                   where (string)result.Attribute("MDLinkIdentity") == (string)result2.Attribute("MDObjId")
//                   from result4 in root.Elements("Class").Elements("MDDefinition")
//                   where (string)result3.Attribute("MDLinkIdentity") == (string)result4.Attribute("MDObjId")
                   

//                   select new Thing
//                   {
//                       type = "activityConsumesResource",
//                       id = (string)result.Parent.Parent.Attribute("MDObjId") + "_2",
//                       name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                       value = (string)result.Parent.Parent.Attribute("MDObjId"),
//                       place1 = (string)result3.Attribute("MDLinkIdentity"),
//                       place2 = (string)result.Attribute("MDLinkIdentity"),
//                       foundation = "CoupleType",
//                       value_type = "$id$"
//                   };

//            tuple_types = tuple_types.Concat(results);
//            aro = results.GroupBy(x=>(string)x.value).ToDictionary(y => y.Key, y => y.ToList());
//            MergeDictionaries(OV5b_aro_mandatory_views, aro);
//            aro2 = results.GroupBy(x => (string)x.value).ToDictionary(y => y.Key, y => y.ToList());
//            MergeDictionaries(OV6c_aro_optional_views, aro2);


//            foreach (Thing thing in results.ToList())
//            {
//                if (things_dic.TryGetValue(thing.place1, out value))
//                {
//                    values = new List<Thing>();
//                    values.Add(value);
//                    OV5b_aro_optional_views.Add((string)thing.value, values);
//                    MergeDictionaries(OV6c_aro_optional_views, new Dictionary<string, List<Thing>>() { { (string)thing.value, values } });
//                }
//            }

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    from result2 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result.Parent.Attribute("MDPrpName") == "consumingActivity"
//                    where (string)result.Attribute("MDLinkIdentity") == (string)result2.Attribute("MDObjId")
//                    select new Thing
//                    {
//                        type = "activityConsumesResource",
//                        id = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                        value = "$none$",
//                        place1 = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        place2 = (string)result.Attribute("MDLinkIdentity"),
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    };

//            foreach (Thing thing in results.ToList())
//            {
//                if (aro.TryGetValue(thing.id, out values))
//                    continue;

//                errors_list.Add("Definition error," + thing.id + "," + thing.name + "," + thing.type + ",Missing Mandatory Element: Resource\r\n");

//                //values = new List<Thing>();

//                //values.Add(new Thing
//                //{
//                //    type = "Resource",
//                //    id = thing.id + "_1",
//                //    name = thing.name,
//                //    value = "$none$",
//                //    place1 = "$none$",
//                //    place2 = "$none$",
//                //    foundation = "IndividualType",
//                //    value_type = "$none$"
//                //});

//                //things = things.Concat(values);
//                //things_dic.Add(values.First().id, values.First());
//                //OV5b_aro_optional_views.Add(thing.id, values);

//                //values = new List<Thing>();

//                //values.Add(new Thing
//                //{
//                //    type = "Resource",
//                //    id = thing.id + "_1",
//                //    name = thing.name,
//                //    value = "$none$",
//                //    place1 = "$none$",
//                //    place2 = "$none$",
//                //    foundation = "IndividualType",
//                //    value_type = "$none$"
//                //});

//                //values.Add(new Thing
//                //{
//                //    type = "activityConsumesResource",
//                //    id = thing.id + "_2",
//                //    name = "ARO",
//                //    value = "$none$",
//                //    place1 = thing.id + "_1",
//                //    place2 = thing.place2,
//                //    foundation = "CoupleType",
//                //    value_type = "$none$"
//                //});

//                //OV6c_aro_optional_views.Add(thing.id, values);

//                //values = new List<Thing>();

//                //values.Add(new Thing
//                //{
//                //    type = "activityConsumesResource",
//                //    id = thing.id + "_2",
//                //    name = "ARO",
//                //    value = "$none$",
//                //    place1 = thing.id + "_1",
//                //    place2 = thing.place2,
//                //    foundation = "CoupleType",
//                //    value_type = "$none$"
//                //});

//                //tuple_types = tuple_types.Concat(values);
//                //OV5b_aro_mandatory_views.Add(thing.id, values);

//            }

//            results =
//                   from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                   where (string)result.Parent.Attribute("MDPrpName") == "producingActivity"
//                   from result3 in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                   where (string)result3.Parent.Attribute("MDPrpName") == "Resources"
//                   where (string)result.Parent.Parent.Attribute("MDObjId") == (string)result3.Parent.Parent.Attribute("MDObjId")
//                   from result4 in root.Elements("Class").Elements("MDDefinition")
//                   where (string)result3.Attribute("MDLinkIdentity") == (string)result4.Attribute("MDObjId")
//                   from result2 in root.Elements("Class").Elements("MDDefinition")
//                   where (string)result.Attribute("MDLinkIdentity") == (string)result2.Attribute("MDObjId")

//                   select new Thing
//                   {
//                       type = "activityProducesResource",
//                       id = (string)result.Parent.Parent.Attribute("MDObjId") + "_3",
//                       name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                       value = (string)result.Parent.Parent.Attribute("MDObjId"),
//                       place2 = (string)result3.Attribute("MDLinkIdentity"),
//                       place1 = (string)result.Attribute("MDLinkIdentity"),
//                       foundation = "CoupleType",
//                       value_type = "$id$"
//                   };

//            tuple_types = tuple_types.Concat(results);
//            aro = results.GroupBy(x => (string)x.value).ToDictionary(y => y.Key, y => y.ToList());
//            MergeDictionaries(OV5b_aro_mandatory_views, aro);
//            aro2 = results.GroupBy(x => (string)x.value).ToDictionary(y => y.Key, y => y.ToList());
//            MergeDictionaries(OV6c_aro_optional_views, aro2);

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    from result2 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result.Parent.Attribute("MDPrpName") == "producingActivity"
//                    where (string)result.Attribute("MDLinkIdentity") == (string)result2.Attribute("MDObjId")
//                    select new Thing
//                    {
//                        type = "activityProducesResource",
//                        id = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                        value = "$none$",
//                        place1 = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        place2 = (string)result.Attribute("MDLinkIdentity"),
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    };

//            foreach (Thing thing in results.ToList())
//            {
//                if (aro.TryGetValue(thing.id, out values))
//                    continue;

//                errors_list.Add("Definition error," + thing.id + "," + thing.name + "," + thing.type + ",Missing Mandatory Element: Resource\r\n");

//                //values = new List<Thing>();

//                //values.Add(new Thing
//                //{
//                //    type = "activityProducesResource",
//                //    id = thing.id + "_3",
//                //    name = "ARO",
//                //    value = "$none$",
//                //    place1 = thing.place2,
//                //    place2 = thing.id + "_1",
//                //    foundation = "CoupleType",
//                //    value_type = "$none$"
//                //});
                
//                //tuple_types = tuple_types.Concat(values);

//                //results_dic = new Dictionary<string, List<Thing>>();

//                //results_dic.Add(thing.id, values);

//                //MergeDictionaries(OV6c_aro_optional_views, results_dic);
//                //MergeDictionaries(OV5b_aro_mandatory_views, results_dic);
//            }

//            //activityChangesResource

//            results_dic =
//                    (from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty")
//                     where (string)result.Attribute("MDPrpName") == "Behavior"
//                     select new
//                     {
//                         key = (string)result.Parent.Attribute("MDObjId"),
//                         value = new List<Thing> {new Thing
//                                            {
//                                                type = "ActivityChangesResource",
//                                                id = (string)result.Parent.Attribute("MDObjId"),
//                                                name = ((string)result.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                                                value = (string)result.Attribute("MDPrpValue"),
//                                                place1 = "$none$",
//                                                place2 = "$none$",
//                                                foundation = "$none$",
//                                                value_type = "$none$"
//                                            }}
//                     }).ToDictionary(a => a.key, a => a.value);

//            if (results_dic.Count() > 0)
//            {
//                results =
//                        from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                        where (string)result.Parent.Attribute("MDPrpName") == "Activity"
//                        from result3 in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                        where (string)result3.Parent.Attribute("MDPrpName") == "Resource"
//                        where (string)result3.Parent.Parent.Attribute("MDObjId") == (string)result.Parent.Parent.Attribute("MDObjId")
//                        from result2 in root.Elements("Class").Elements("MDDefinition")
//                        where (string)result.Attribute("MDLinkIdentity") == (string)result2.Attribute("MDObjId")
//                        from result4 in root.Elements("Class").Elements("MDDefinition")
//                        where (string)result3.Attribute("MDLinkIdentity") == (string)result4.Attribute("MDObjId")
//                        select new Thing
//                        {
//                            type = (((string)(results_dic[(string)result.Parent.Parent.Attribute("MDObjId")].First().value) == "Consumes") ? "activityConsumesResource" : "activityProducesResource"),
//                            id = (string)result.Parent.Parent.Attribute("MDObjId"),
//                            name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                            value = "$none$",
//                            place1 = (((string)(results_dic[(string)result.Parent.Parent.Attribute("MDObjId")].First().value) == "Consumes") ? (string)result3.Attribute("MDLinkIdentity") : (string)result.Attribute("MDLinkIdentity")),
//                            place2 = (((string)(results_dic[(string)result.Parent.Parent.Attribute("MDObjId")].First().value) == "Consumes") ? (string)result.Attribute("MDLinkIdentity") : (string)result3.Attribute("MDLinkIdentity")),
//                            foundation = "CoupleType",
//                            value_type = "$none$"
//                        };

//                tuple_types = tuple_types.Concat(results.ToList());

//                values_dic = results.ToDictionary(x => x.id, x => x);
//                foreach (Thing thing in results_dic.Select(x=>x.Value.First()).ToList())
//                {
//                    if (!values_dic.TryGetValue(thing.id, out value))
//                    {
//                        errors_list.Add("Definition error," + thing.id + "," + thing.name + "," + thing.type + ",Missing Mandatory Element: Resource\r\n");
//                    }
//                }
//            }

//            //activityPerformedByPerformer

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result.Parent.Attribute("MDPrpName") == "Activity"
//                    from result3 in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result3.Parent.Attribute("MDPrpName") == "Performer"
//                    where (string)result3.Parent.Parent.Attribute("MDObjId") == (string)result.Parent.Parent.Attribute("MDObjId")
//                    from result2 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result.Attribute("MDLinkIdentity") == (string)result2.Attribute("MDObjId")
//                    from result4 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result3.Attribute("MDLinkIdentity") == (string)result4.Attribute("MDObjId")


//                    select new Thing
//                    {
//                        type = "activityPerformedByPerformer",
//                        id = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                        value = "$none$",
//                        place1 = (string)result3.Attribute("MDLinkIdentity"),
//                        place2 = (string)result.Attribute("MDLinkIdentity"),
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    };

//            tuple_types = tuple_types.Concat(results.ToList());

//            values_dic = results.ToDictionary(a => a.id, a => a);

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result.Parent.Attribute("MDPrpName") == "Activity"
//                    from result3 in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result3.Parent.Attribute("MDPrpName") == "Performer"
//                    where (string)result3.Parent.Parent.Attribute("MDObjId") == (string)result.Parent.Parent.Attribute("MDObjId")
//                    //from result2 in root.Elements("Class").Elements("MDDefinition")
//                    //from result4 in root.Elements("Class").Elements("MDDefinition")
//                    //where (string)result.Attribute("MDLinkIdentity") == (string)result2.Attribute("MDObjId")
//                    //where (string)result3.Attribute("MDLinkIdentity") == (string)result4.Attribute("MDObjId")


//                    select new Thing
//                    {
//                        type = "activityPerformedByPerformer",
//                        id = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                        value = (string)result3.Parent.Attribute("MDPrpValue"),
//                        place1 = (string)result3.Attribute("MDLinkIdentity"),
//                        place2 = (string)result.Attribute("MDLinkIdentity"),
//                        foundation = "CoupleType",
//                        value_type = "$view name$"
//                    };

//            values = new List<Thing>();
//            values2 = new List<Thing>();

//            foreach (Thing thing in results)
//            {

//                if (!values_dic.TryGetValue(thing.id, out value))
//                {
//                    //    values2.Add(thing);

//                    if (!things_dic.TryGetValue(thing.place2, out value))
//                    {
//                        errors_list.Add(thing.id + "," + thing.name + "," + thing.type + ",Missing Mandatory Element: Activity\r\n");
//                    //    value = new Thing
//                    //    {
//                    //        type = "Activity",
//                    //        id = thing.place2,
//                    //        name = thing.name,
//                    //        value = "$none$",
//                    //        place1 = "$none$",
//                    //        place2 = "$none$",
//                    //        foundation = "IndividualType",
//                    //        value_type = "$none$"
//                    //    };
//                    //    values.Add(value);
//                    //    things_dic.Add(thing.place2, value);
//                    }

//                    if (!things_dic.TryGetValue(thing.place1, out value))
//                    {
//                        if (((string)thing.value).Contains("Service"))
//                        {
//                            errors_list.Add("Definition error," + thing.id + "," + thing.name + "," + thing.type + ",Missing Mandatory Element: Service\r\n");
//                    //        value = new Thing
//                    //        {
//                    //            type = "Service",
//                    //            id = thing.place1,
//                    //            name = thing.name,
//                    //            value = "$none$",
//                    //            place1 = "$none$",
//                    //            place2 = "$none$",
//                    //            foundation = "IndividualType",
//                    //            value_type = "$none$"
//                    //        };
//                    //        values.Add(value);
//                    //        things_dic.Add(thing.place1, value);
//                        }
//                        else if (((string)thing.value).Contains("System"))
//                        {
//                            errors_list.Add("Definition error," + thing.id + "," + thing.name + "," + thing.type + ",Missing Mandatory Element: System\r\n");
//                    //        value = new Thing
//                    //        {
//                    //            type = "System",
//                    //            id = thing.place1,
//                    //            name = thing.name,
//                    //            value = "$none$",
//                    //            place1 = "$none$",
//                    //            place2 = "$none$",
//                    //            foundation = "IndividualType",
//                    //            value_type = "$none$"
//                    //        };
//                    //        values.Add(value);
//                    //        things_dic.Add(thing.place1, value);
//                        }
//                        else
//                        {
//                            errors_list.Add("Definition error," + thing.id + "," + thing.name + "," + thing.type + ",Missing Mandatory Element: Performer\r\n");
//                    //        value = new Thing
//                    //        {
//                    //            type = "Performer",
//                    //            id = thing.place1,
//                    //            name = thing.name,
//                    //            value = "$none$",
//                    //            place1 = "$none$",
//                    //            place2 = "$none$",
//                    //            foundation = "IndividualType",
//                    //            value_type = "$none$"
//                    //        };
//                    //        values.Add(value);
//                    //        things_dic.Add(thing.place1, value);
//                        }
//                    }
//                }
//            }

//            //things = things.Concat(values);

//            //tuple_types = tuple_types.Concat(values2);

//            //Needlines

//            //values = new List<Thing>();

//            //values_dic = things_dic.Where(x => x.Value.type == "Resource" || x.Value.type == "Data").ToDictionary(p => p.Key, p => p.Value);
//            ////var acr3 = tuple_types.Where(x => x.type == "activityConsumesResource").Where(x => temp_dic.ContainsKey(x.place1)).GroupBy(x => x.place2).Where(x => x.Count() == 1).Select(grp => grp.First());
//            //values_dic2 = tuple_types.Where(x => x.type == "activityConsumesResource").Where(x => values_dic.ContainsKey(x.place1)).GroupBy(x => x.place2).Where(x => x.Count() == 1).ToDictionary(y => y.Key, y => y.First());
//            //results = tuple_types.Where(x => x.type == "activityPerformedByPerformer").GroupBy(x =>x.place2).Where(x => x.Count() == 1).Select(grp => grp.First());
//            ////var app4 = app3.GroupBy(x =>x.place1).Where(x => x.Count() == 1).ToDictionary(y => y.Key, y => y.First());

//            //values_dic.Clear();
//            //foreach (Thing rela in results)
//            //{
//            //    if (values_dic2.TryGetValue(rela.place2, out value))
//            //    {
//            //        if(!values_dic.Remove(rela.place1))
//            //            values_dic.Add(rela.place1, rela);
//            //    }

//            //}

//            //values_dic.Clear();
//            //foreach (Thing rela in acr3)
//            //{
//            //    if (app4.TryGetValue(rela.place2, out value))
//            //    {
//            //        values_dic.Add(value.place1, value);
//            //    }

//            //}

//            results_dic = tuple_types.Where(x => x.type == "activityPerformedByPerformer").GroupBy(x => x.place1).ToDictionary(x => x.Key, x => x.ToList());
//            values_dic = tuple_types.Where(x => x.type == "activityProducesResource").GroupBy(x => x.place1).Select(x=>x.First()).ToDictionary(x => x.place1, x => x);
//            values_dic2 = tuple_types.Where(x => x.type == "activityConsumesResource").GroupBy(x => x.place2).Select(x => x.First()).ToDictionary(x => x.place2, x => x);

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    from result2 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result.Parent.Parent.Attribute("MDObjMinorTypeName") != "System Exchange (DM2rx)" && (string)result.Parent.Parent.Attribute("MDObjMinorTypeName") != "Operational Exchange (DM2rx)"
//                        && (string)result.Parent.Parent.Attribute("MDObjMinorTypeName") != "System Data Flow (DM2rx)" && (string)result.Parent.Parent.Attribute("MDObjMinorTypeName") != "Service Data Flow (DM2rx)"
//                    where (string)result.Parent.Attribute("MDPrpName") == "performerTarget" || (string)result.Parent.Attribute("MDPrpName") == "Target" || (string)result.Parent.Attribute("MDPrpName") == "Destination" 
//                    where (string)result.Attribute("MDLinkIdentity") == (string)result2.Attribute("MDObjId")
//                    select new Thing
//                    {
//                        type = "Resource Flow",
//                        id = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                        value = (string)result.Parent.Attribute("MDPrpName") + "_" + (string)result.Parent.Parent.Attribute("MDObjMinorTypeName"),
//                        place1 = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        place2 = (string)result.Attribute("MDLinkIdentity"),
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    };

//            foreach (Thing thing in results.ToList())
//            {
//                if (results_dic.TryGetValue(thing.place2, out values))
//                {
//                    add = true;
//                    foreach (Thing app in values)
//                    {
//                        if(values_dic2.TryGetValue(app.place2,out value))
//                        {
//                            add = false;
//                            break;
//                        }
//                    }
//                    if(add)
//                        errors_list.Add("Definition error," + thing.id + "," + thing.name + "," + thing.type + ",Missing Mandatory Element: activityConsumesResource\r\n");   
//                }
//                else
//                {
//                    errors_list.Add("Definition error," + thing.id + "," + thing.name + "," + thing.type + ",Missing Mandatory Element: activityPerformedByPerformer\r\n");
//                }

//                //if (values_dic.TryGetValue(thing.id, out value))
//                //    continue;

//                //values = new List<Thing>();
                        
//                //values.Add(new Thing
//                //    {
//                //        type = "Activity",
//                //        id = thing.id + "_1",
//                //        name = thing.name,
//                //        value = "$none$",
//                //        place1 = "$none$",
//                //        place2 = "$none$",
//                //        foundation = "IndividualType",
//                //        value_type = "$none$"
//                //    });
//                //values.Add(new Thing
//                //{
//                //    type = (((string)thing.value).Contains("System") || ((string)thing.value).Contains("Service") ? "Data" : "Resource"),
//                //    id = thing.id + "_2",
//                //    name = (((string)thing.value).Contains("performerTarget") ? "Needline" : "SF"),
//                //    value = "$none$",
//                //    place1 = "$none$",
//                //    place2 = "$none$",
//                //    foundation = "IndividualType",
//                //    value_type = "$none$"
//                //});

//                //things = things.Concat(values);

//                //values = new List<Thing>();

//                //values.Add(new Thing
//                //{
//                //    type = "activityPerformedByPerformer",
//                //    id = thing.id,
//                //    name = thing.name,
//                //    value = "$none$",
//                //    place1 = thing.place2,
//                //    place2 = thing.id + "_1",
//                //    foundation = "CoupleType",
//                //    value_type = "$none$"
//                //});

//                //values.Add(new Thing
//                //{
//                //    type = "activityConsumesResource",
//                //    id = thing.id + "_3",
//                //    name = (((string)thing.value).Contains("performerTarget") ? "Needline" : "SF"),
//                //    value = "$none$",
//                //    place1 = thing.id + "_2",
//                //    place2 = thing.id + "_1",
//                //    foundation = "CoupleType",
//                //    value_type = "$none$"
//                //});

//                //tuple_types = tuple_types.Concat(values);

//                //values = new List<Thing>();

//                //values.Add(new Thing
//                //{
//                //    type = (((string)thing.value).Contains("System") || ((string)thing.value).Contains("Service") ? "Data" : "Resource"),
//                //    id = thing.id + "_2",
//                //    name = (((string)thing.value).Contains("performerTarget") ? "Needline" : "SF"),
//                //    value = "$none$",
//                //    place1 = "$none$",
//                //    place2 = "$none$",
//                //    foundation = "IndividualType",
//                //    value_type = "$none$"
//                //});

//                //if (!((string)thing.value).Contains("Service") && !((string)thing.value).Contains("System"))
//                //{
//                //    needline_optional_views.Add(thing.id, values);
//                //    values = new List<Thing>();
//                //}

//                //values.Add(new Thing
//                //{
//                //    type = "activityPerformedByPerformer",
//                //    id = thing.id,
//                //    name = thing.name,
//                //    value = "$none$",
//                //    place1 = thing.place2,
//                //    place2 = thing.id + "_1",
//                //    foundation = "CoupleType",
//                //    value_type = "$none$"
//                //});

//                //values.Add(new Thing
//                //{
//                //    type = "activityConsumesResource",
//                //    id = thing.id + "_3",
//                //    name = (((string)thing.value).Contains("performerTarget") ? "Needline" : "SF"),
//                //    value = "$none$",
//                //    place1 = thing.id + "_2",
//                //    place2 = thing.id + "_1",
//                //    foundation = "CoupleType",
//                //    value_type = "$none$"
//                //});

//                //values.Add(new Thing
//                //{
//                //    type = "Activity",
//                //    id = thing.id + "_1",
//                //    name = thing.name,
//                //    value = "$none$",
//                //    place1 = "$none$",
//                //    place2 = "$none$",
//                //    foundation = "IndividualType",
//                //    value_type = "$none$"
//                //});

//                //needline_mandatory_views.Add(thing.id, values);
//            }

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result.Parent.Parent.Attribute("MDObjMinorTypeName") != "System Exchange (DM2rx)" && (string)result.Parent.Parent.Attribute("MDObjMinorTypeName") != "Operational Exchange (DM2rx)"
//                        && (string)result.Parent.Parent.Attribute("MDObjMinorTypeName") != "System Data Flow (DM2rx)" && (string)result.Parent.Parent.Attribute("MDObjMinorTypeName") != "Service Data Flow (DM2rx)"
//                    where (string)result.Parent.Attribute("MDPrpName") == "performerSource" || (string)result.Parent.Attribute("MDPrpName") == "Source"
//                    from result2 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result.Attribute("MDLinkIdentity") == (string)result2.Attribute("MDObjId")
//                    select new Thing
//                    {
//                        type = "Resource Flow",
//                        id = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                        value = (string)result.Parent.Attribute("MDPrpName") + "_" + (string)result.Parent.Parent.Attribute("MDObjMinorTypeName"),
//                        place1 = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        place2 = (string)result.Attribute("MDLinkIdentity"),
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    };

//            foreach (Thing thing in results.ToList())
//            {

//                if (results_dic.TryGetValue(thing.place2, out values))
//                {
//                    add = true;
//                    foreach (Thing app in values)
//                    {
//                        if (values_dic.TryGetValue(app.place2, out value))
//                        {
//                            add = false;
//                            break;
//                        }
//                    }
//                    if (add)
//                        errors_list.Add("Definition error," + thing.id + "," + thing.name + "," + thing.type + ",Missing Mandatory Element: activityProducesResource\r\n");
//                }
//                else
//                {
//                    errors_list.Add("Definition error," + thing.id + "," + thing.name + "," + thing.type + ",Missing Mandatory Element: activityPerformedByPerformer\r\n");
//                }

//                //values = new List<Thing>();

//                //values.Add(new Thing
//                //{
//                //    type = "Activity",
//                //    id = thing.id + "_4",
//                //    name = thing.name,
//                //    value = "$none$",
//                //    place1 = "$none$",
//                //    place2 = "$none$",
//                //    foundation = "IndividualType",
//                //    value_type = "$none$"
//                //});

//                //things = things.Concat(values);

//                //values = new List<Thing>();

//                //values.Add(new Thing
//                //{
//                //    type = "activityProducesResource",
//                //    id = thing.id + "_5",
//                //    name = (((string)thing.value).Contains("performerSource") ? "Needline" : "SF"),
//                //    value = "$none$",
//                //    place1 = thing.id + "_4",
//                //    place2 = thing.id + "_2",
//                //    foundation = "CoupleType",
//                //    value_type = "$none$"
//                //});

//                //values.Add(new Thing
//                //{
//                //    type = "activityPerformedByPerformer",
//                //    id = thing.id + "_6",
//                //    name = thing.name,
//                //    value = "$none$",
//                //    place1 = thing.place2,
//                //    place2 = thing.id + "_4",
//                //    foundation = "CoupleType",
//                //    value_type = "$none$"
//                //});

//                //tuple_types = tuple_types.Concat(values);

//                //values = new List<Thing>();

//                //values.Add(new Thing
//                //{
//                //    type = "activityProducesResource",
//                //    id = thing.id + "_5",
//                //    name = (((string)thing.value).Contains("performerSource") ? "Needline" : "SF"),
//                //    value = "$none$",
//                //    place1 = thing.id + "_4",
//                //    place2 = thing.id + "_2",
//                //    foundation = "CoupleType",
//                //    value_type = "$none$"
//                //});

//                //values.Add(new Thing
//                //{
//                //    type = "Activity",
//                //    id = thing.id + "_4",
//                //    name = thing.name,
//                //    value = "$none$",
//                //    place1 = "$none$",
//                //    place2 = "$none$",
//                //    foundation = "IndividualType",
//                //    value_type = "$none$"
//                //});

//                //values.Add(new Thing
//                //{
//                //    type = "activityPerformedByPerformer",
//                //    id = thing.id + "_6",
//                //    name = thing.name,
//                //    value = "$none$",
//                //    place1 = thing.place2,
//                //    place2 = thing.id + "_4",
//                //    foundation = "CoupleType",
//                //    value_type = "$none$"
//                //});

//                //results_dic = new Dictionary<string, List<Thing>>();

//                //results_dic.Add(thing.id, values);

//                //MergeDictionaries(needline_mandatory_views, results_dic);
//            }

//            //Supports

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    from result2 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result.Parent.Attribute("MDPrpName") == "SupportedBy"
//                    where (string)result.Attribute("MDLinkIdentity") == (string)result2.Attribute("MDObjId")
//                    select new Thing
//                    {
//                        type = "activityPerformedByPerformer",
//                        id = (string)result.Parent.Parent.Attribute("MDObjId") + (string)result.Attribute("MDLinkIdentity"),
//                        name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                        value = "$none$",
//                        place1 = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        place2 = (string)result.Attribute("MDLinkIdentity"),
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    };

//            foreach (Thing thing in results.ToList())
//            {
//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "Activity",
//                    id = thing.id + "_2",
//                    name = thing.name,
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "Resource",
//                    id = thing.id + "_3",
//                    name = "Support",
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                things = things.Concat(values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "activityPerformedByPerformer",
//                    id = thing.id + "_1",
//                    name = thing.name,
//                    value = "$none$",
//                    place1 = thing.id,
//                    place2 = thing.id + "_2",
//                    foundation = "CoupleType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "activityConsumesResource",
//                    id = thing.id + "_4",
//                    name = "Support",
//                    value = "$none$",
//                    place1 = thing.id + "_3",
//                    place2 = thing.id + "_2",
//                    foundation = "CoupleType",
//                    value_type = "$none$"
//                });

//                tuple_types = tuple_types.Concat(values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "Resource",
//                    id = thing.id + "_3",
//                    name = "Support",
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });
//                if (!OV2_support_optional_views.ContainsKey(thing.id))
//                    OV2_support_optional_views.Add(thing.id, values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "activityPerformedByPerformer",
//                    id = thing.id + "_1",
//                    name = thing.name,
//                    value = "$none$",
//                    place1 = thing.id,
//                    place2 = thing.id + "_2",
//                    foundation = "CoupleType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "activityConsumesResource",
//                    id = thing.id + "_4",
//                    name = "Support",
//                    value = "$none$",
//                    place1 = thing.id + "_3",
//                    place2 = thing.id + "_2",
//                    foundation = "CoupleType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "Activity",
//                    id = thing.id + "_2",
//                    name = thing.name,
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });
//                if (!OV2_support_mandatory_views.ContainsKey(thing.id))
//                    OV2_support_mandatory_views.Add(thing.id, values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "activityPerformedByPerformer",
//                    id = thing.id + "_1",
//                    name = thing.name,
//                    value = "$none$",
//                    place1 = thing.id,
//                    place2 = thing.id + "_2",
//                    foundation = "CoupleType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "activityConsumesResource",
//                    id = thing.id + "_4",
//                    name = "Support",
//                    value = "$none$",
//                    place1 = thing.id + "_3",
//                    place2 = thing.id + "_2",
//                    foundation = "CoupleType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "Activity",
//                    id = thing.id + "_2",
//                    name = thing.name,
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "Resource",
//                    id = thing.id + "_3",
//                    name = "Support",
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });
//                if (!OV4_support_optional_views.ContainsKey(thing.id))
//                    OV4_support_optional_views.Add(thing.id, values);
//            }

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    from result2 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result.Parent.Attribute("MDPrpName") == "Supports"
//                    where (string)result.Attribute("MDLinkIdentity") == (string)result2.Attribute("MDObjId")
//                    select new Thing
//                    {
//                        type = "activityPerformedByPerformer",
//                        id = (string)result.Parent.Parent.Attribute("MDObjId") + (string)result.Attribute("MDLinkIdentity"),
//                        name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                        value = "$none$",
//                        place1 = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        place2 = (string)result.Attribute("MDLinkIdentity"),
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    };

//            foreach (Thing thing in results.ToList())
//            {
//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "Activity",
//                    id = thing.id + "_5",
//                    name = thing.name,
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                things = things.Concat(values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "activityProducesResource",
//                    id = thing.id + "_6",
//                    name = "Support",
//                    value = "$none$",
//                    place1 = thing.id + "_5",
//                    place2 = thing.place2 + thing.place1 + "_3",
//                    foundation = "CoupleType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "activityPerformedByPerformer",
//                    id = thing.id + "_1",
//                    name = thing.name,
//                    value = "$none$",
//                    place1 = thing.id,
//                    place2 = thing.id + "_5",
//                    foundation = "CoupleType",
//                    value_type = "$none$"
//                });

//                tuple_types = tuple_types.Concat(values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "activityProducesResource",
//                    id = thing.id + "_6",
//                    name = "Support",
//                    value = "$none$",
//                    place1 = thing.id + "_5",
//                    place2 = thing.place2 + thing.place1 + "_3",
//                    foundation = "CoupleType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "Activity",
//                    id = thing.id + "_5",
//                    name = thing.name,
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "activityPerformedByPerformer",
//                    id = thing.id + "_1",
//                    name = thing.name,
//                    value = "$none$",
//                    place1 = thing.id,
//                    place2 = thing.id + "_5",
//                    foundation = "CoupleType",
//                    value_type = "$none$"
//                });

//                if (!OV4_support_optional_views_2.ContainsKey(thing.id))
//                    OV4_support_optional_views_2.Add(thing.id, values);

//                if (!OV2_support_mandatory_views_2.ContainsKey(thing.id))
//                    OV2_support_mandatory_views_2.Add(thing.id, values);

//            }

//            MergeDictionaries(OV4_support_optional_views, OV4_support_optional_views_2);

//            MergeDictionaries(OV2_support_mandatory_views, OV2_support_mandatory_views_2);

//            tuple_types = tuple_types.Distinct();

//            things = things.Distinct();

//            //Constraint

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty")
//                    where (string)result.Attribute("MDPrpName") == "To Cardinality"

//                    select new Thing
//                    {
//                        type = "Rule",
//                        id = (string)result.Parent.Attribute("MDObjId"),
//                        name = (string)result.Attribute("MDPrpValue"),
//                        value = "$none$",
//                        place1 = "$none$",
//                        place2 = "$none$",
//                        foundation = "IndividualType",
//                        value_type = "$none$"
//                    };

//            things = things.Concat(results.ToList());

//            //DIV-3 Relationship

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result.Parent.Parent.Attribute("MDObjMinorTypeName") == "Constraint"
//                    where (string)result.Parent.Attribute("MDPrpName") == "From Table"
//                    from result3 in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result3.Parent.Attribute("MDPrpName") == "To Table"
//                    where (string)result3.Parent.Parent.Attribute("MDObjId") == (string)result.Parent.Parent.Attribute("MDObjId")
//                    from result5 in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result5.Parent.Attribute("MDPrpName") == "Foreign Keys and Roles"
//                    where (string)result5.Parent.Parent.Attribute("MDObjId") == (string)result.Parent.Parent.Attribute("MDObjId")
//                    from result2 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result.Attribute("MDLinkIdentity") == (string)result2.Attribute("MDObjId")
//                    from result4 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result3.Attribute("MDLinkIdentity") == (string)result4.Attribute("MDObjId")
//                    from result6 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result5.Attribute("MDLinkIdentity") == (string)result6.Attribute("MDObjId")

//                    select new Thing
//                    {
//                        type = "temp",
//                        id = (string)result.Parent.Parent.Attribute("MDObjId") + (string)result5.Attribute("MDLinkIdentity"),
//                        name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                        value = (string)result5.Attribute("MDLinkIdentity"),
//                        place1 = (string)result.Attribute("MDLinkIdentity"),
//                        place2 = (string)result3.Attribute("MDLinkIdentity"),
//                        foundation = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        value_type = "$FK ID$"
//                    };

//            values3 = results.ToList();
//            values_dic = values3.GroupBy(x => x.foundation).Select(grp => grp.First()).ToDictionary(x => x.foundation, x => x);

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result.Parent.Parent.Attribute("MDObjMinorTypeName") == "Constraint"
//                    where (string)result.Parent.Attribute("MDPrpName") == "From Table"
//                    from result3 in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result3.Parent.Attribute("MDPrpName") == "To Table"
//                    where (string)result3.Parent.Parent.Attribute("MDObjId") == (string)result.Parent.Parent.Attribute("MDObjId")
//                    from result2 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result.Attribute("MDLinkIdentity") == (string)result2.Attribute("MDObjId")
//                    from result4 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result3.Attribute("MDLinkIdentity") == (string)result4.Attribute("MDObjId")

//                    select new Thing
//                    {
//                        type = "temp",
//                        id = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                        value = "$none$",
//                        place1 = (string)result.Attribute("MDLinkIdentity"),
//                        place2 = (string)result3.Attribute("MDLinkIdentity"),
//                        foundation = "$none$",
//                        value_type = "$PK ID$"
//                    };

//            foreach (Thing thing in results)
//            {
//                if(!values_dic.TryGetValue(thing.id,out value))
//                    values3.Add(thing);
//            }

//            foreach (Thing thing in values3)
//            {
//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "Activity",
//                    id = thing.id + "_2",
//                    name = thing.name,
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                things = things.Concat(values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "activityPerformedByPerformer",
//                    id = thing.id + "_1",
//                    name = thing.name,
//                    value = "$none$",
//                    place1 = thing.place1,
//                    place2 = thing.id + "_2",
//                    foundation = "CoupleType",
//                    value_type = "$none$"
//                });

//                if ((string)thing.value == "$none$")
//                {
//                    values2 = new List<Thing>();

//                    values2.Add(new Thing
//                    {
//                        type = "Data",
//                        id = thing.id + "_9",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = "$none$",
//                        place2 = "$none$",
//                        foundation = "IndividualType",
//                        value_type = "$none$"
//                    });

//                    things = things.Concat(values2);

//                    if (thing.foundation == "$none$")
//                    {
//                        if (!DIV3_mandatory.ContainsKey(thing.id))
//                            DIV3_mandatory.Add(thing.id, values2);
//                    }
//                    else
//                    {
//                        if (!DIV3_mandatory.ContainsKey(thing.foundation))
//                            DIV3_mandatory.Add(thing.foundation, values2);
//                    }

//                    values.Add(new Thing
//                    {
//                        type = "activityProducesResource",
//                        id = thing.id + "_3",
//                        name = thing.name,
//                        value = "$none$",
//                        place2 = thing.id + "_9",
//                        place1 = thing.id + "_2",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });
//                }
//                else
//                {
//                    values.Add(new Thing
//                    {
//                        type = "activityProducesResource",
//                        id = thing.id + "_3",
//                        name = thing.name,
//                        value = "$none$",
//                        place2 = (string)thing.value,
//                        place1 = thing.id + "_2",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });
//                }

//                if (thing.foundation == "$none$")
//                    values.Add(new Thing
//                    {
//                        type = "ruleConstrainsActivity",
//                        id = thing.id + "_7",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = thing.id,
//                        place2 = thing.id + "_2",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });
//                else
//                    values.Add(new Thing
//                    {
//                        type = "ruleConstrainsActivity",
//                        id = thing.id + "_7",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = thing.foundation,
//                        place2 = thing.id + "_2",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });

//                tuple_types = tuple_types.Concat(values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "activityPerformedByPerformer",
//                    id = thing.id + "_1",
//                    name = thing.name,
//                    value = "$none$",
//                    place1 = thing.place1,
//                    place2 = thing.id + "_2",
//                    foundation = "CoupleType",
//                    value_type = "$none$"
//                });

//                if ((string)thing.value == "$none$")
//                {
//                //    values.Add(new Thing
//                //    {
//                //        type = "Data",
//                //        id = thing.id + "_9",
//                //        name = thing.name,
//                //        value = "$none$",
//                //        place1 = "$none$",
//                //        place2 = "$none$",
//                //        foundation = "IndividualType",
//                //        value_type = "$none$"
//                //    });

//                    values.Add(new Thing
//                    {
//                        type = "activityProducesResource",
//                        id = thing.id + "_3",
//                        name = thing.name,
//                        value = "$none$",
//                        place2 = thing.id + "_9",
//                        place1 = thing.id + "_2",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });
//                }
//                else
//                {
//                    values.Add(new Thing
//                    {
//                        type = "activityProducesResource",
//                        id = thing.id + "_3",
//                        name = thing.name,
//                        value = "$none$",
//                        place2 = (string)thing.value,
//                        place1 = thing.id + "_2",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });
//                }

//                if (thing.foundation == "$none$")
//                    values.Add(new Thing
//                    {
//                        type = "ruleConstrainsActivity",
//                        id = thing.id + "_7",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = thing.id,
//                        place2 = thing.id + "_2",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });
//                else
//                    values.Add(new Thing
//                    {
//                        type = "ruleConstrainsActivity",
//                        id = thing.id + "_7",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = thing.foundation,
//                        place2 = thing.id + "_2",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });

//                values.Add(new Thing
//                {
//                    type = "Activity",
//                    id = thing.id + "_2",
//                    name = thing.name,
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });
//                if (thing.foundation == "$none$")
//                {
//                    if (!DIV3_optional.ContainsKey(thing.id))
//                        DIV3_optional.Add(thing.id, values);
//                }
//                else
//                {
//                    if (!DIV3_optional.ContainsKey(thing.foundation))
//                        DIV3_optional.Add(thing.foundation, values);
//                }
//            }

//            foreach (Thing thing in values3)
//            {
//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "Activity",
//                    id = thing.id + "_4",
//                    name = thing.name,
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                things = things.Concat(values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "activityPerformedByPerformer",
//                    id = thing.id + "_5",
//                    name = thing.name,
//                    value = "$none$",
//                    place1 = thing.place2,
//                    place2 = thing.id + "_4",
//                    foundation = "CoupleType",
//                    value_type = "$none$"
//                });

//                if ((string)thing.value == "$none$")
//                {
//                    //values2 = new List<Thing>();

//                    //values2.Add(new Thing
//                    //{
//                    //    type = "Data",
//                    //    id = thing.id + "_9",
//                    //    name = "Support",
//                    //    value = "$none$",
//                    //    place1 = "$none$",
//                    //    place2 = "$none$",
//                    //    foundation = "IndividualType",
//                    //    value_type = "$none$"
//                    //});

//                    //things = things.Concat(values2);

//                    values.Add(new Thing
//                    {
//                        type = "activityConsumesResource",
//                        id = thing.id + "_6",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = thing.id + "_9",
//                        place2 = thing.id + "_4",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });
//                }
//                else
//                {
//                    values.Add(new Thing
//                    {
//                        type = "activityConsumesResource",
//                        id = thing.id + "_6",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = (string)thing.value,
//                        place2 = thing.id + "_4",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });
//                }

//                if (thing.foundation == "$none$")
//                    values.Add(new Thing
//                    {
//                        type = "ruleConstrainsActivity",
//                        id = thing.id + "_8",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = thing.id,
//                        place2 = thing.id + "_4",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });
//                else
//                    values.Add(new Thing
//                    {
//                        type = "ruleConstrainsActivity",
//                        id = thing.id + "_8",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = thing.foundation,
//                        place2 = thing.id + "_4",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });

//                tuple_types = tuple_types.Concat(values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "activityPerformedByPerformer",
//                    id = thing.id + "_5",
//                    name = thing.name,
//                    value = "$none$",
//                    place1 = thing.place2,
//                    place2 = thing.id + "_4",
//                    foundation = "CoupleType",
//                    value_type = "$none$"
//                });

//                if ((string)thing.value == "$none$")
//                {
//                    //values.Add(new Thing
//                    //{
//                    //    type = "Data",
//                    //    id = thing.id + "_9",
//                    //    name = "Support",
//                    //    value = "$none$",
//                    //    place1 = "$none$",
//                    //    place2 = "$none$",
//                    //    foundation = "IndividualType",
//                    //    value_type = "$none$"
//                    //});

//                    values.Add(new Thing
//                    {
//                        type = "activityConsumesResource",
//                        id = thing.id + "_6",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = thing.id + "_9",
//                        place2 = thing.id + "_4",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });
//                }
//                else
//                {
//                    values.Add(new Thing
//                    {
//                        type = "activityConsumesResource",
//                        id = thing.id + "_6",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = (string)thing.value,
//                        place2 = thing.id + "_4",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });
//                }

//                if (thing.foundation == "$none$")
//                    values.Add(new Thing
//                    {
//                        type = "ruleConstrainsActivity",
//                        id = thing.id + "_8",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = thing.foundation,
//                        place2 = thing.id + "_4",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });
//                else
//                    values.Add(new Thing
//                    {
//                        type = "ruleConstrainsActivity",
//                        id = thing.id + "_8",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = (string)thing.value,
//                        place2 = thing.id + "_4",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });

//                values.Add(new Thing
//                {
//                    type = "Activity",
//                    id = thing.id + "_4",
//                    name = thing.name,
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });
          
//                 MergeDictionaries(DIV3_optional, new Dictionary<string,List<Thing>>(){{thing.id,values}});
//            }

//            //State transition

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result.Parent.Attribute("MDPrpName") == "To"
//                    from result3 in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result3.Parent.Attribute("MDPrpName") == "From"
//                    where (string)result3.Parent.Parent.Attribute("MDObjId") == (string)result.Parent.Parent.Attribute("MDObjId")
//                    from result2 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result.Attribute("MDLinkIdentity") == (string)result2.Attribute("MDObjId")
//                    from result4 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result3.Attribute("MDLinkIdentity") == (string)result4.Attribute("MDObjId")


//                    select new Thing
//                    {
//                        type = "BeforeAfterType",
//                        id = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                        value = "$none$",
//                        place1 = (string)result3.Attribute("MDLinkIdentity"),
//                        place2 = (string)result.Attribute("MDLinkIdentity"),
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    };

//            tuple_types = tuple_types.Concat(results.ToList());

//            //Capability Dependency

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result.Parent.Attribute("MDPrpName") == "To Capability"
//                    from result3 in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result3.Parent.Attribute("MDPrpName") == "From Capability"
//                    where (string)result3.Parent.Parent.Attribute("MDObjId") == (string)result.Parent.Parent.Attribute("MDObjId")
//                    from result2 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result.Attribute("MDLinkIdentity") == (string)result2.Attribute("MDObjId")
//                    from result4 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result3.Attribute("MDLinkIdentity") == (string)result4.Attribute("MDObjId")


//                    select new Thing
//                    {
//                        type = "BeforeAfterType",
//                        id = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                        value = "$none$",
//                        place1 = (string)result3.Attribute("MDLinkIdentity"),
//                        place2 = (string)result.Attribute("MDLinkIdentity"),
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    };

//            tuple_types = tuple_types.Concat(results.ToList());

//            //System Milestone Dependency

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result.Parent.Attribute("MDPrpName") == "To Milestone"
//                    from result3 in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result3.Parent.Attribute("MDPrpName") == "From Milestone"
//                    where (string)result3.Parent.Parent.Attribute("MDObjId") == (string)result.Parent.Parent.Attribute("MDObjId")
//                    from result2 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result.Attribute("MDLinkIdentity") == (string)result2.Attribute("MDObjId")
//                    from result4 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result3.Attribute("MDLinkIdentity") == (string)result4.Attribute("MDObjId")


//                    select new Thing
//                    {
//                        type = "BeforeAfterType",
//                        id = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                        value = "$none$",
//                        place1 = (string)result3.Attribute("MDLinkIdentity"),
//                        place2 = (string)result.Attribute("MDLinkIdentity"),
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    };

//            tuple_types = tuple_types.Concat(results.ToList());

//            //activityPartOfProjectType

//            results =
//                        from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                        where (string)result.Parent.Attribute("MDPrpName") == "Milestones"
//                        select new Thing
//                        {
//                            type = "activityPartOfProjectType",
//                            id = (string)result.Parent.Parent.Attribute("MDObjId") + (string)result.Attribute("MDLinkIdentity"),
//                            name = "$none$",
//                            value = "$none$",
//                            place1 = (string)result.Parent.Parent.Attribute("MDObjId"),
//                            place2 = (string)result.Attribute("MDLinkIdentity"),
//                            foundation = "WholePartType",
//                            value_type = "$none$"
//                        };

//                values = results.ToList();

//                results =
//                            from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                            where (string)result.Parent.Attribute("MDPrpName") == "Project"
//                            select new Thing
//                            {
//                                type = "activityPartOfProjectType",
//                                id = (string)result.Attribute("MDLinkIdentity") + (string)result.Parent.Parent.Attribute("MDObjId"),
//                                name = "$none$",
//                                value = "$none$",
//                                place2 = (string)result.Parent.Parent.Attribute("MDObjId"),
//                                place1 = (string)result.Attribute("MDLinkIdentity"),
//                                foundation = "WholePartType",
//                                value_type = "$none$"
//                            };

//                values.AddRange(results.ToList());

//            tuple_types = tuple_types.Concat(values.GroupBy(x => x.id).Select(grp => grp.First()));

//            //activityPerformedByPerformer - Milestones

//            results =
//                        from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                        where (string)result.Parent.Attribute("MDPrpName") == "Milestones"
//                        select new Thing
//                        {
//                            type = "activityPerformedByPerformer",
//                            id = (string)result.Parent.Parent.Attribute("MDObjId") + (string)result.Attribute("MDLinkIdentity"),
//                            name = "$none$",
//                            value = "$none$",
//                            place1 = (string)result.Parent.Parent.Attribute("MDObjId"),
//                            place2 = (string)result.Attribute("MDLinkIdentity"),
//                            foundation = "CoupleType",
//                            value_type = "$none$"
//                        };

//            values = results.ToList();

//            results =
//                        from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                        where (string)result.Parent.Attribute("MDPrpName") == "In Item"
//                        select new Thing
//                        {
//                            type = "activityPerformedByPerformer",
//                            id = (string)result.Attribute("MDLinkIdentity") + (string)result.Parent.Parent.Attribute("MDObjId"),
//                            name = "$none$",
//                            value = "$none$",
//                            place2 = (string)result.Parent.Parent.Attribute("MDObjId"),
//                            place1 = (string)result.Attribute("MDLinkIdentity"),
//                            foundation = "CoupleType",
//                            value_type = "$none$"
//                        };

//            values.AddRange(results.ToList());

//            tuple_types = tuple_types.Concat(values.GroupBy(x => x.id).Select(grp => grp.First()));

//            //Milestone Date

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty")
//                    where (string)result.Attribute("MDPrpName") == "Milestone Date"

//                    select new Thing
//                    {
//                        type = "HappensInType",
//                        id = (string)result.Parent.Attribute("MDObjId") + "_t2",
//                        name = "$none$",
//                        value = (string)result.Attribute("MDPrpValue"),
//                        place1 = (string)result.Parent.Attribute("MDObjId") + "_t1",
//                        place2 = (string)result.Parent.Attribute("MDObjId"),
//                        foundation = "WholePartType",
//                        value_type = "$period$"
//                    };

//            tuple_types = tuple_types.Concat(results.ToList());

//            foreach (Thing thing in results)
//            {
//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "PeriodType",
//                    id = thing.place2 + "_t1",
//                    name = (string)thing.value,
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                things = things.Concat(values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "PeriodType",
//                    id = thing.place2 + "_t1",
//                    name = (string)thing.value,
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                values.Add(thing);

//                period_dic.Add(thing.place2, values);
//            }

//            //Data Type

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty")
//                    where (string)result.Attribute("MDPrpName") == "SQL Data Type"

//                    select new Thing
//                    {
//                        type = "typeInstance",
//                        id = (string)result.Parent.Attribute("MDObjId") + "_12",
//                        name = "$none$",
//                        value = (string)result.Attribute("MDPrpValue"),
//                        place1 = (string)result.Parent.Attribute("MDObjId") + "_11",
//                        place2 = (string)result.Parent.Attribute("MDObjId"),
//                        foundation = "typeInstance",
//                        value_type = "$datatype$"
//                    };

//            tuples = tuples.Concat(results.ToList());

//            foreach (Thing thing in results)
//            {
//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "DataType",
//                    id = thing.place2 + "_11",
//                    name = (string)thing.value,
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                things = things.Concat(values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "DataType",
//                    id = thing.place2 + "_11",
//                    name = (string)thing.value,
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                //values.Add(thing);

//                datatype_optional_dic.Add(thing.place2, new List<Thing>(){thing});
//                datatype_mandatory_dic.Add(thing.place2, values);
//            }

//            //activityPartOfCapability

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result.Parent.Attribute("MDPrpName") == "Activity"
//                    from result3 in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result3.Parent.Attribute("MDPrpName") == "Capability"
//                    where (string)result3.Parent.Parent.Attribute("MDObjId") == (string)result.Parent.Parent.Attribute("MDObjId")
//                    from result2 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result.Attribute("MDLinkIdentity") == (string)result2.Attribute("MDObjId")
//                    from result4 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result3.Attribute("MDLinkIdentity") == (string)result4.Attribute("MDObjId")
                    

//                    select new Thing
//                    {
//                        type = "activityPartOfCapability",
//                        id = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                        value = "$none$",
//                        place1 = (string)result3.Attribute("MDLinkIdentity"),
//                        place2 = (string)result.Attribute("MDLinkIdentity"),
//                        foundation = "WholePartType",
//                        value_type = "$none$"
//                    };

//            tuple_types = tuple_types.Concat(results.ToList());

//            //DIV-2 Relationship

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result.Parent.Attribute("MDPrpName") == "From Entity"
//                    from result3 in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result3.Parent.Attribute("MDPrpName") == "To Entity"
//                    where (string)result3.Parent.Parent.Attribute("MDObjId") == (string)result.Parent.Parent.Attribute("MDObjId")
//                    from result2 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result.Attribute("MDLinkIdentity") == (string)result2.Attribute("MDObjId")
//                    from result4 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result3.Attribute("MDLinkIdentity") == (string)result4.Attribute("MDObjId")


//                    select new Thing
//                    {
//                        type = "OverlapType",
//                        id = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                        value = "$none$",
//                        place1 = (string)result3.Attribute("MDLinkIdentity"),
//                        place2 = (string)result.Attribute("MDLinkIdentity"),
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    };

//            tuple_types = tuple_types.Concat(results.ToList());

//            //things_dic

//            things_dic = things.ToDictionary(x => x.id, x => x);

//            //System Exchange (DM2rx)

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result.Parent.Attribute("MDPrpName") == "Source"
//                    from result3 in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result3.Parent.Attribute("MDPrpName") == "Target"
//                    where (string)result3.Parent.Parent.Attribute("MDObjId") == (string)result.Parent.Parent.Attribute("MDObjId")
//                    where (string)result.Parent.Parent.Attribute("MDObjMinorTypeName") == "System Exchange (DM2rx)"

//                    select new Thing
//                    {
//                        type = "temp",
//                        id = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                        value = "$none$",
//                        place1 = (string)result.Attribute("MDLinkIdentity"),
//                        place2 = (string)result3.Attribute("MDLinkIdentity"),
//                        foundation = "$none$",
//                        value_type = "$none$"
//                    };

//            values_dic = tuple_types.Where(x => x.type == "activityPerformedByPerformer").ToDictionary(x => x.id, x => x);

//            foreach (Thing thing in results)
//            {
//                values = new List<Thing>();
//                values2 = new List<Thing>();
//                mandatory_list  = new List<Thing>();

//                values_dic.TryGetValue(thing.place1, out value);

//                values.Add(new Thing
//                {
//                    type = "activityProducesResource",
//                    id = thing.place1 + "_1",
//                    name = thing.name,
//                    value = "$none$",
//                    place1 = thing.id,
//                    place2 = value.place2,
//                    foundation = "CoupleType",
//                    value_type = "$none$"
//                });

//                mandatory_list.Add(value);
//                mandatory_list.Add(things_dic[value.place1]);
//                mandatory_list.Add(things_dic[value.place2]);

//                values2.Add(new Thing
//                {
//                    type = "Data",
//                    id = thing.id,
//                    name = "Resource",
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                things_dic.Add(thing.id, new Thing
//                {
//                    type = "Data",
//                    id = thing.id,
//                    name = "Resource",
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });


//                values_dic.TryGetValue(thing.place2, out value);

//                values.Add(new Thing
//                {
//                    type = "activityConsumesResource",
//                    id = thing.id + "_2",
//                    name = thing.name,
//                    value = "$none$",
//                    place1 = value.place2,
//                    place2 = thing.id,
//                    foundation = "CoupleType",
//                    value_type = "$none$"
//                });


//                mandatory_list.Add(value);
//                mandatory_list.Add(things_dic[value.place1]);
//                mandatory_list.Add(things_dic[value.place2]);
//                mandatory_list.AddRange(values);
//                mandatory_list.AddRange(values2);

//                needline_mandatory_views.Add(thing.id, mandatory_list);
//            }

//            things = things.Concat(values2);

//            tuple_types = tuple_types.Concat(values);

//            //System Data Flow (DM2rx)

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result.Parent.Attribute("MDPrpName") == "Source"
//                    where (string)result.Parent.Parent.Attribute("MDObjMinorTypeName") == "System Data Flow (DM2rx)"
//                    from result2 in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result2.Parent.Attribute("MDPrpName") == "Destination"
//                    where (string)result2.Parent.Parent.Attribute("MDObjId") == (string)result.Parent.Parent.Attribute("MDObjId")
//                    from result3 in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result3.Parent.Attribute("MDPrpName") == "Resources"
//                    where (string)result3.Parent.Parent.Attribute("MDObjId") == (string)result2.Parent.Parent.Attribute("MDObjId")
//                    from result4 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result3.Attribute("MDLinkIdentity") == (string)result4.Attribute("MDObjId")

//                    select new Thing
//                    {
//                        type = "temp",
//                        id = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                        value = (string)result3.Attribute("MDLinkIdentity"),
//                        place1 = (string)result.Attribute("MDLinkIdentity"),
//                        place2 = (string)result2.Attribute("MDLinkIdentity"),
//                        //foundation = (string)result4.Attribute("MDObjId"),
//                        value_type = "$resources$"
//                    };

//            //results2 = tuple_types.Where(x => x.type == "activityPerformedByPerformer");//.GroupBy(x => x.place2).Where(x => x.Count() == 1).Select(grp => grp.First());
//            //values_dic = results2.GroupBy(x =>x.place2).Where(x => x.Count() == 1).ToDictionary(y => y.Key, y => y.First());

//            foreach (Thing thing in results)
//            {
//                values = new List<Thing>();
//                values2 = new List<Thing>();
//                mandatory_list = new List<Thing>();
//                bool needs_data = false;
 
//                if (things_dic.TryGetValue((string)thing.value, out value2))
//                {
//                    if (value2.type != "Data")
//                    {
//                        values2.Add(new Thing
//                        {
//                            type = "Data",
//                            id = thing.id + "_7",
//                            name = thing.name,
//                            value = "$none$",
//                            place1 = "$none$",
//                            place2 = "$none$",
//                            foundation = "IndividualType",
//                            value_type = "$none$"
//                        });

//                        things_dic.Add(thing.id + "_7", new Thing
//                        {
//                            type = "Data",
//                            id = thing.id + "_7",
//                            name = thing.name,
//                            value = "$none$",
//                            place1 = "$none$",
//                            place2 = "$none$",
//                            foundation = "IndividualType",
//                            value_type = "$none$"
//                        });

//                        needs_data = true;
//                    }
//                    else
//                    {
//                        mandatory_list.Add(value2);
//                    }
//                }

//                things_dic.TryGetValue(thing.place1, out value);

//                if (value.type != "Activity")// || !values_dic.TryGetValue(thing.place1, out value2))
//                {
//                    values2.Add(new Thing
//                    {
//                        type = "Activity",
//                        id = thing.id + "_1",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = "$none$",
//                        place2 = "$none$",
//                        foundation = "IndividualType",
//                        value_type = "$none$"
//                    });

//                    things_dic.Add(thing.id + "_1", new Thing
//                    {
//                        type = "Activity",
//                        id = thing.id + "_1",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = "$none$",
//                        place2 = "$none$",
//                        foundation = "IndividualType",
//                        value_type = "$none$"
//                    });

//                    values.Add(new Thing
//                    {
//                        type = "activityProducesResource",
//                        id = thing.id + "_3",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = thing.id + "_1",
//                        place2 = (needs_data ? thing.id + "_7" : (string)thing.value),
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });

//                    values.Add(new Thing
//                    {
//                        type = "activityPerformedByPerformer",
//                        id = thing.id + "_5",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = thing.place1,
//                        place2 = thing.id + "_1",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });

//                }
//                else
//                {
//                    values.Add(new Thing
//                    {
//                        type = "activityProducesResource",
//                        id = thing.id + "_3",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = thing.place1,
//                        place2 = (needs_data ? thing.id + "_7" : (string)thing.value),
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });
//                }

//                things_dic.TryGetValue(thing.place2, out value);

//                if (value.type != "Activity")// || !values_dic.TryGetValue(thing.place2, out value2))
//                {
//                    values2.Add(new Thing
//                    {
//                        type = "Activity",
//                        id = thing.id + "_2",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = "$none$",
//                        place2 = "$none$",
//                        foundation = "IndividualType",
//                        value_type = "$none$"
//                    });

//                    things_dic.Add(thing.id + "_2", new Thing
//                    {
//                        type = "Activity",
//                        id = thing.id + "_2",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = "$none$",
//                        place2 = "$none$",
//                        foundation = "IndividualType",
//                        value_type = "$none$"
//                    });

//                    values.Add(new Thing
//                    {
//                        type = "activityConsumesResource",
//                        id = thing.id + "_4",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = (needs_data ? thing.id + "_7" : (string)thing.value),
//                        place2 = thing.id + "_2",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });

//                    values.Add(new Thing
//                    {
//                        type = "activityPerformedByPerformer",
//                        id = thing.id + "_6",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = thing.place2,
//                        place2 = thing.id + "_2",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });
//                }
//                else
//                {
//                    values.Add(new Thing
//                    {
//                        type = "activityConsumesResource",
//                        id = thing.id + "_4",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = (needs_data ? thing.id + "_7" : (string)thing.value),
//                        place2 = thing.place2,
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });
//                }

//                mandatory_list.AddRange(values);
//                if (values2.Count>0)
//                    mandatory_list.AddRange(values2);

//                needline_mandatory_views.Add(thing.id, mandatory_list);
            
//            }
            
//            if (values2.Count > 0)
//                things = things.Concat(values2);

//            tuple_types = tuple_types.Concat(values);

//            //Service Data Flow (DM2rx)

//            results =
//                    from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result.Parent.Attribute("MDPrpName") == "Source"
//                    where (string)result.Parent.Parent.Attribute("MDObjMinorTypeName") == "Service Data Flow (DM2rx)"
//                    from result2 in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result2.Parent.Attribute("MDPrpName") == "Destination"
//                    where (string)result2.Parent.Parent.Attribute("MDObjId") == (string)result.Parent.Parent.Attribute("MDObjId")
//                    from result3 in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                    where (string)result3.Parent.Attribute("MDPrpName") == "Resources"
//                    where (string)result3.Parent.Parent.Attribute("MDObjId") == (string)result2.Parent.Parent.Attribute("MDObjId")
//                    from result4 in root.Elements("Class").Elements("MDDefinition")
//                    where (string)result3.Attribute("MDLinkIdentity") == (string)result4.Attribute("MDObjId")

//                    select new Thing
//                    {
//                        type = "temp",
//                        id = (string)result.Parent.Parent.Attribute("MDObjId"),
//                        name = ((string)result.Parent.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                        value = (string)result3.Attribute("MDLinkIdentity"),
//                        place1 = (string)result.Attribute("MDLinkIdentity"),
//                        place2 = (string)result2.Attribute("MDLinkIdentity"),
//                        //foundation = (string)result4.Attribute("MDObjId"),
//                        value_type = "$resources$"
//                    };

//            //results2 = tuple_types.Where(x => x.type == "activityPerformedByPerformer");//.GroupBy(x => x.place2).Where(x => x.Count() == 1).Select(grp => grp.First());
//            //values_dic = results2.GroupBy(x =>x.place2).Where(x => x.Count() == 1).ToDictionary(y => y.Key, y => y.First());

//            foreach (Thing thing in results)
//            {
//                values = new List<Thing>();
//                values2 = new List<Thing>();
//                mandatory_list = new List<Thing>();
//                bool needs_data = false;

//                if (things_dic.TryGetValue((string)thing.value, out value2))
//                {
//                    if (value2.type != "Data")
//                    {
//                        values2.Add(new Thing
//                        {
//                            type = "Data",
//                            id = thing.id + "_7",
//                            name = thing.name,
//                            value = "$none$",
//                            place1 = "$none$",
//                            place2 = "$none$",
//                            foundation = "IndividualType",
//                            value_type = "$none$"
//                        });

//                        things_dic.Add(thing.id + "_7", new Thing
//                        {
//                            type = "Data",
//                            id = thing.id + "_7",
//                            name = thing.name,
//                            value = "$none$",
//                            place1 = "$none$",
//                            place2 = "$none$",
//                            foundation = "IndividualType",
//                            value_type = "$none$"
//                        });

//                        needs_data = true;
//                    }
//                    else
//                    {
//                        mandatory_list.Add(value2);
//                    }
//                }

//                things_dic.TryGetValue(thing.place1, out value);

//                if (value.type != "Activity")// || !values_dic.TryGetValue(thing.place1, out value2))
//                {
//                    values2.Add(new Thing
//                    {
//                        type = "Activity",
//                        id = thing.id + "_1",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = "$none$",
//                        place2 = "$none$",
//                        foundation = "IndividualType",
//                        value_type = "$none$"
//                    });

//                    things_dic.Add(thing.id + "_1", new Thing
//                    {
//                        type = "Activity",
//                        id = thing.id + "_1",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = "$none$",
//                        place2 = "$none$",
//                        foundation = "IndividualType",
//                        value_type = "$none$"
//                    });

//                    values.Add(new Thing
//                    {
//                        type = "activityProducesResource",
//                        id = thing.id + "_3",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = thing.id + "_1",
//                        place2 = (needs_data ? thing.id + "_7" : (string)thing.value),
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });

//                    values.Add(new Thing
//                    {
//                        type = "activityPerformedByPerformer",
//                        id = thing.id + "_5",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = thing.place1,
//                        place2 = thing.id + "_1",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });

//                }
//                else
//                {
//                    values.Add(new Thing
//                    {
//                        type = "activityProducesResource",
//                        id = thing.id + "_3",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = thing.place1,
//                        place2 = (needs_data ? thing.id + "_7" : (string)thing.value),
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });
//                }

//                things_dic.TryGetValue(thing.place2, out value);

//                if (value.type != "Activity")// || !values_dic.TryGetValue(thing.place2, out value2))
//                {
//                    values2.Add(new Thing
//                    {
//                        type = "Activity",
//                        id = thing.id + "_2",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = "$none$",
//                        place2 = "$none$",
//                        foundation = "IndividualType",
//                        value_type = "$none$"
//                    });

//                    things_dic.Add(thing.id + "_2", new Thing
//                    {
//                        type = "Activity",
//                        id = thing.id + "_2",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = "$none$",
//                        place2 = "$none$",
//                        foundation = "IndividualType",
//                        value_type = "$none$"
//                    });

//                    values.Add(new Thing
//                    {
//                        type = "activityConsumesResource",
//                        id = thing.id + "_4",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = (needs_data ? thing.id + "_7" : (string)thing.value),
//                        place2 = thing.id + "_2",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });

//                    values.Add(new Thing
//                    {
//                        type = "activityPerformedByPerformer",
//                        id = thing.id + "_6",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = thing.place2,
//                        place2 = thing.id + "_2",
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });
//                }
//                else
//                {
//                    values.Add(new Thing
//                    {
//                        type = "activityConsumesResource",
//                        id = thing.id + "_4",
//                        name = thing.name,
//                        value = "$none$",
//                        place1 = (needs_data ? thing.id + "_7" : (string)thing.value),
//                        place2 = thing.place2,
//                        foundation = "CoupleType",
//                        value_type = "$none$"
//                    });
//                }

//                mandatory_list.AddRange(values);
//                if (values2.Count > 0)
//                    mandatory_list.AddRange(values2);

//                needline_mandatory_views.Add(thing.id, mandatory_list);

//            }

//            if (values2.Count > 0)
//                things = things.Concat(values2);

//            tuple_types = tuple_types.Concat(values);

//            //Organization Owns Projects and PV-1

//            //results =
//            //            from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//            //            where (string)result.Parent.Attribute("MDPrpName") == "Organization Owns Projects"
//            //            from result2 in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//            //            where (string)result2.Parent.Attribute("MDPrpName") == "Milestones"
//            //            where (string)result.Attribute("MDLinkIdentity") == (string)result2.Parent.Parent.Attribute("MDObjId")

//            //            select new Thing
//            //            {
//            //                type = "activityPerformedByPerformer",
//            //                id = (string)result2.Attribute("MDLinkIdentity") + (string)result.Parent.Parent.Attribute("MDObjId"),
//            //                name = ((string)result2.Attribute("MDLinkName")).Replace("&quot", "").Replace("\"",""),
//            //                value = new Tuple<string, string>((string)result.Parent.Parent.Parent.Element("MDDiagram").Attribute("MDObjId"), (string)result2.Parent.Parent.Attribute("MDObjId")),
//            //                place2 = (string)result.Parent.Parent.Attribute("MDObjId"),
//            //                place1 = (string)result2.Attribute("MDLinkIdentity"),
//            //                foundation = "CoupleType",
//            //                value_type = "$view and project$"
//            //            };

//            //    IEnumerable<Thing> tuple_types_temp = results.ToList();

//            results =
//                        from result in root.Elements("Class").Elements("MDDefinition").Elements("MDProperty").Elements("MDLink")
//                        where (string)result.Parent.Attribute("MDPrpName") == "Project Owned By Organization"
//                        from result2 in result.Parent.Parent.Elements("MDProperty").Elements("MDLink")
//                        where (string)result2.Parent.Attribute("MDPrpName") == "Milestones"

//                        select new Thing
//                        {
//                            type = "activityPerformedByPerformer",
//                            id = (string)result2.Attribute("MDLinkIdentity") + (string)result.Attribute("MDLinkIdentity"),
//                            name = ((string)result2.Attribute("MDLinkName")).Replace("&quot","").Replace("\"",""),
//                            //value = new Tuple<string, string>((string)result.Parent.Parent.Parent.Element("MDDiagram").Attribute("MDObjId"), (string)result.Parent.Parent.Attribute("MDObjId")),
//                            value = (string)result.Parent.Parent.Attribute("MDObjId"),
//                            place2 = (string)result.Attribute("MDLinkIdentity"),
//                            place1 = (string)result2.Attribute("MDLinkIdentity"),
//                            foundation = "CoupleType",
//                            value_type = "$view and project$"
//                        };

//            if (results.Count() > 0)
//            {
//                IEnumerable<Thing> tuple_types_temp = results.ToList();

//                //tuple_types_temp = tuple_types_temp.Concat(results.ToList());

//                //tuple_types_temp = tuple_types_temp.GroupBy(x => x.id).Select(grp => grp.First());

//                tuple_types = tuple_types.Concat(tuple_types_temp);

//                results2 = tuple_types_temp;

//                foreach (Thing thing in tuple_types_temp)
//                {
//                    if (things_dic.TryGetValue(thing.place1, out value))
//                    {
//                        results2 = results2.Concat(new[] { value });
//                    }
//                    else
//                    {
//                        values = new List<Thing>();

//                        values.Add(new Thing
//                        {
//                            type = "Activity",
//                            id = thing.place1,
//                            name = thing.name,
//                            value = thing.value,
//                            place2 = "$none$",
//                            place1 = "$none$",
//                            foundation = "IndividualType",
//                            value_type = "$view and project$"

//                        });

//                        things = things.Concat(values);

//                        things_dic.Add(thing.place1, values.First());

//                        results2 = results2.Concat(values);

//                        values = new List<Thing>();

//                        values.Add(new Thing
//                        {
//                            type = "activityPartOfProjectType",
//                            id = (string)thing.value + thing.place1,
//                            name = "$none$",
//                            value = thing.value,
//                            place1 = (string)thing.value,
//                            place2 = thing.place1,
//                            foundation = "IndividualType",
//                            value_type = "$view and project$"

//                        });

//                        tuple_types = tuple_types.Concat(values);

//                        results2 = results2.Concat(values);
//                    }
//                }

//                results =
//                           from result in root.Elements("Class").Elements("MDDiagram").Elements("MDSymbol")
//                           where (string)result.Parent.Attribute("MDObjMinorTypeName") == "PV-01 Project Portfolio Relationships (DM2)" || (string)result.Parent.Attribute("MDObjMinorTypeName") == "PV-01 Project Portfolio Relationships At Time (DM2)"
//                           where (string)result.Attribute("MDObjMinorTypeName") == "Project (DM2)"

//                           select new Thing
//                           {
//                               type = "temp",
//                               id = (string)result.Parent.Attribute("MDObjId") + "_" + (string)result.Attribute("MDSymIdDef"),
//                               place1 = (string)result.Parent.Attribute("MDObjId"),
//                               place2 = (string)result.Attribute("MDSymIdDef")
//                           };

//                results_dic = results2.GroupBy(y => (string)y.value).ToDictionary(z => z.Key, z => z.ToList());

//                sorted_results = results.GroupBy(x => x.place1).Select(group => group.Distinct().ToList()).ToList();

//                foreach (List<Thing> view in sorted_results)
//                {
//                    values2 = new List<Thing>();

//                    foreach (Thing thing in view)
//                    {
//                        values = new List<Thing>();

//                        if (results_dic.TryGetValue(thing.place2, out values))
//                        {
//                            values2.AddRange(values);
//                        }
//                    }
//                    PV1_mandatory_views.Add(view.First().place1, values2);
//                }
//            }
//                    //PV1_mandatory_views = tuple_types_temp_new.GroupBy(y => ((Tuple<string, string>)y.value).Item1).ToDictionary(z => ((Tuple<string, string>)z.First().value).Item1, z => z.ToList());

//            //ToLists
//                    values3 = tuples.ToList();
//                    values4 = tuple_types.ToList();
//                    things = null;
//                    tuples = null;
//                    tuple_types = null;

//            //AV-2

//            sorted_results = new List<List<Thing>>();
//            optional_list = new List<Thing>();
//            values = new List<Thing>();

//            foreach (Thing thing in things_dic.Select(kvp => kvp.Value).ToList().OrderBy(x => x.type).ToList())
//            {
//                optional_list.Add(new Thing { id = thing.id, type = thing.type, value = "$none$", value_type = "$none$"});
//                values.Add(new Thing { id = "_1", type = "AV-2", place2 = thing.id, value = thing.type, place1 = "_1"});
//            }

//            sorted_results.Add(values);

//            sorted_results_new = new List<List<Thing>>();
//            Add_Tuples(ref sorted_results, ref sorted_results_new, values3, ref errors_list);
//            Add_Tuples(ref sorted_results, ref sorted_results_new, values4, ref errors_list);
//            sorted_results = sorted_results_new;

//            foreach (Thing thing in sorted_results.First())
//            {
//                if ((string)thing.value == "superSubtype" || (string)thing.value == "WholePartType")
//                    optional_list.Add(new Thing { id = thing.place2, type = (string)thing.value, value = "$none$", value_type = "$none$" });
//            }

//            views.Add(new View { type = "AV-2", id = "_1", name = "NEAR AV-2", optional = optional_list, mandatory = new List<Thing>() });

//            ////OV-3

//            //mandatory_list = new List<Thing>();
//            //values = new List<Thing>();
//            //optional_list = new List<Thing>();
//            //sorted_results = new List<List<Thing>>();

//            //values_dic2 = things_dic.Where(x => x.Value.type == "Resource").ToDictionary(p => p.Key, p => p.Value);

//            //results = values4.Where(x => x.type == "activityConsumesResource").Where(x => values_dic2.ContainsKey(x.place1));
//            //values_dic = values4.Where(x => x.type == "activityProducesResource").GroupBy(x => x.place2).Where(x => x.Count() == 1).Select(grp => grp.First()).ToDictionary(x => x.place2, x=>x);

//            //foreach (Thing rela in results)
//            //{
//            //    if(values_dic.TryGetValue(rela.place1, out value))
//            //    {
//            //        values.Add(rela);
//            //        values.Add(value);
//            //    }

//            //}

//            //count = 0;
//            //count2 = values.Count();

//            ////var duplicateKeys = app2.GroupBy(x => x.place2)
//            ////            .Where(group => group.Count() > 1)
//            ////            .Select(group => group.Key);

//            ////List<string> test = duplicateKeys.ToList();

//            //values_dic2 = values4.Where(x => x.type == "activityPerformedByPerformer").Where(x => Allowed_Element("OV-3", x.place1, ref things_dic)).GroupBy(x => x.place2).Select(grp => grp.First()).ToDictionary(x => x.place2, x => x);

//            //while (count < count2)
//            //{
//            //    add = false;

//            //    foreach (Thing thing in values)
//            //    {
//            //        if (values_dic2.TryGetValue(values[count].place2, out value))
//            //            if (values_dic2.TryGetValue(values[count + 1].place1, out value2))
//            //            {
//            //                add = true;
//            //                values.Add(value);
//            //                values.Add(value2);
//            //                break;
//            //            }
//            //    }


//            //    if (add == true)
//            //    {
//            //        count = count + 2;
//            //    }
//            //    else
//            //    {
//            //        values.RemoveAt(count);
//            //        values.RemoveAt(count);
//            //        count2 = count2 - 2;
//            //    }
//            //}

//            //sorted_results.Add(Add_Places(things_dic, values));

//            //foreach (Thing thing in sorted_results.First())
//            //{
//            //    temp = Find_Mandatory_Optional(thing.type, "OV-3", "OV-3", "_2", ref errors_list);
//            //    if (temp == "Mandatory")
//            //        mandatory_list.Add(new Thing { id = thing.id, type = thing.type, value = "$none$", value_type = "$none$" });
//            //    if (temp == "Optional")
//            //        optional_list.Add(new Thing { id = thing.id, type = thing.type, value = "$none$", value_type = "$none$" });
//            //}

//            //if (sorted_results.First().Count() > 0)
//            //    views.Add(new View { type = "OV-3", id = "_2", name = "NEAR OV-3", optional = optional_list, mandatory = mandatory_list });

//            //SV-6

//            mandatory_list = new List<Thing>();
//            values = new List<Thing>();
//            optional_list = new List<Thing>();
//            sorted_results = new List<List<Thing>>();

//            values_dic2 = things_dic.Where(x => x.Value.type == "Data").ToDictionary(p => p.Key, p => p.Value);

//            results = values4.Where(x => x.type == "activityConsumesResource").Where(x => values_dic2.ContainsKey(x.place1));
//            values_dic = values4.Where(x => x.type == "activityProducesResource").GroupBy(x => x.place2).Where(x => x.Count() == 1).Select(grp => grp.First()).ToDictionary(x => x.place2, x => x);

//            foreach (Thing rela in results)
//            {
//                if (values_dic.TryGetValue(rela.place1, out value))
//                {
//                    values.Add(rela);
//                    values.Add(value);
//                }

//            }

//            count = 0;
//            count2 = values.Count();

//            //var duplicateKeys = app2.GroupBy(x => x.place2)
//            //            .Where(group => group.Count() > 1)
//            //            .Select(group => group.Key);

//            //List<string> test = duplicateKeys.ToList();

//            values_dic2 = values4.Where(x => x.type == "activityPerformedByPerformer").Where(x => Allowed_Element("SV-6", x.place1, ref things_dic)).GroupBy(x => x.place2).Select(grp => grp.First()).ToDictionary(x => x.place2, x => x);

//            while (count < count2)
//            {
//                add = false;

//                foreach (Thing thing in values)
//                {
//                    if (values_dic2.TryGetValue(values[count].place2, out value))
//                        if (values_dic2.TryGetValue(values[count + 1].place1, out value2))
//                        {
//                            add = true;
//                            values.Add(value);
//                            values.Add(value2);
//                            break;
//                        }
//                }


//                if (add == true)
//                {
//                    count = count + 2;
//                }
//                else
//                {
//                    values.RemoveAt(count);
//                    values.RemoveAt(count);
//                    count2 = count2 - 2;
//                }
//            }

//            sorted_results.Add(Add_Places(things_dic, values));

//            foreach (Thing thing in sorted_results.First())
//            {
//                temp = Find_Mandatory_Optional(thing.type, "SV-6", "SV-6", "_3", ref errors_list);
//                if (temp == "Mandatory")
//                    mandatory_list.Add(new Thing { id = thing.id, type = thing.type, value = "$none$", value_type = "$none$" });
//                if (temp == "Optional")
//                    optional_list.Add(new Thing { id = thing.id, type = thing.type, value = "$none$", value_type = "$none$" });
//            }

//            if (sorted_results.First().Count() > 0)
//                views.Add(new View { type = "SV-6", id = "_3", name = "NEAR SV-6", optional = optional_list, mandatory = mandatory_list });

//            //SvcV-6

//            mandatory_list = new List<Thing>();
//            values = new List<Thing>();
//            optional_list = new List<Thing>();
//            sorted_results = new List<List<Thing>>();

//            values_dic2 = things_dic.Where(x => x.Value.type == "Data").ToDictionary(p => p.Key, p => p.Value);

//            results = values4.Where(x => x.type == "activityConsumesResource").Where(x => values_dic2.ContainsKey(x.place1));
//            values_dic = values4.Where(x => x.type == "activityProducesResource").GroupBy(x => x.place2).Where(x => x.Count() == 1).Select(grp => grp.First()).ToDictionary(x => x.place2, x => x);

//            foreach (Thing rela in results)
//            {
//                if (values_dic.TryGetValue(rela.place1, out value))
//                {
//                    values.Add(rela);
//                    values.Add(value);
//                }

//            }

//            count = 0;
//            count2 = values.Count();

//            //var duplicateKeys = app2.GroupBy(x => x.place2)
//            //            .Where(group => group.Count() > 1)
//            //            .Select(group => group.Key);

//            //List<string> test = duplicateKeys.ToList();

//            values_dic2 = values4.Where(x => x.type == "activityPerformedByPerformer").Where(x => Allowed_Element("SvcV-6", x.place1, ref things_dic)).GroupBy(x => x.place2).Select(grp => grp.First()).ToDictionary(x => x.place2, x => x);

//            while (count < count2)
//            {
//                add = false;

//                foreach (Thing thing in values)
//                {
//                    if (values_dic2.TryGetValue(values[count].place2, out value))
//                        if (values_dic2.TryGetValue(values[count + 1].place1, out value2))
//                        {
//                            add = true;
//                            values.Add(value);
//                            values.Add(value2);
//                            break;
//                        }
//                }


//                if (add == true)
//                {
//                    count = count + 2;
//                }
//                else
//                {
//                    values.RemoveAt(count);
//                    values.RemoveAt(count);
//                    count2 = count2 - 2;
//                }
//            }

//            sorted_results.Add(Add_Places(things_dic, values));

//            foreach (Thing thing in sorted_results.First())
//            {

//                temp = Find_Mandatory_Optional(thing.type, "SvcV-6", "SvcV-6", "_4", ref errors_list);
//                if (temp == "Mandatory")
//                    mandatory_list.Add(new Thing { id = thing.id, type = thing.type, value = "$none$", value_type = "$none$" });
//                if (temp == "Optional")
//                    optional_list.Add(new Thing { id = thing.id, type = thing.type, value = "$none$", value_type = "$none$" });

//                if ((string)thing.type == "Service")
//                {
//                    values = new List<Thing>();

//                    values.Add(new Thing
//                    {
//                        type = "ServiceDescription",
//                        id = thing.place2 + "_2",
//                        name = thing.place2 + "_Description",
//                        value = "$none$",
//                        place1 = "$none$",
//                        place2 = "$none$",
//                        foundation = "Individual",
//                        value_type = "$none$"
//                    });

//                    values.Add(new Thing
//                    {
//                        type = "serviceDescribedBy",
//                        id = thing.place2 + "_1",
//                        name = "$none$",
//                        value = "$none$",
//                        place1 = thing.id,
//                        place2 = thing.id + "_2",
//                        foundation = "namedBy",
//                        value_type = "$none$"
//                    });

//                    mandatory_list.AddRange(values);
//                }
//            }

//            if (sorted_results.First().Count() > 0)
//                views.Add(new View { type = "SvcV-6", id = "_4", name = "NEAR SvcV-6", optional = optional_list, mandatory = mandatory_list });

//            //DIV-3 Parts

//            results_dic = values4.Where(x => x.type == "WholePartType").GroupBy(x => x.place1).ToDictionary(gdc => gdc.Key, gdc => gdc.ToList());

//            //Diagramming

//            locations =
//                    from result in root.Elements("Class").Elements("MDDiagram").Elements("MDSymbol")
//                    where (string)result.Attribute("MDSymIdDef") != null
//                        || (string)result.Attribute("MDObjMinorTypeName") == "Picture" || (string)result.Attribute("MDObjMinorTypeName") == "Doc Block"
//                    select new Location
//                    {
//                        id = ((string)result.Attribute("MDObjMinorTypeName") == "Picture" || (string)result.Attribute("MDObjMinorTypeName") == "Doc Block") ? (string)result.Parent.Attribute("MDObjId") + (string)result.Attribute("MDObjId") : (string)result.Parent.Attribute("MDObjId") + (string)result.Attribute("MDObjId"),
//                        top_left_x = (string)result.Attribute("MDSymLocX"),
//                        top_left_y = (string)result.Attribute("MDSymLocY"),
//                        bottom_right_x = ((int)result.Attribute("MDSymLocX") + (int)result.Attribute("MDSymSizeX")).ToString(),
//                        bottom_right_y = ((int)result.Attribute("MDSymLocY") - (int)result.Attribute("MDSymSizeY")).ToString(),
//                        element_id = ((string)result.Attribute("MDObjMinorTypeName") == "Picture" || (string)result.Attribute("MDObjMinorTypeName") == "Doc Block") ? (string)result.Attribute("MDObjId") : (string)result.Attribute("MDSymIdDef")
//                    };

//            foreach (Location location in locations)
//            {
//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "Information",
//                    id = location.id + "_12",
//                    name = "Diagramming Information",
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "Point",
//                    id = location.id + "_16",
//                    name = "Top Left Location",
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "PointType",
//                    id = location.id + "_14",
//                    name = "Top Left LocationType",
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "Point",
//                    id = location.id + "_26",
//                    name = "Bottome Right Location",
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "PointType",
//                    id = location.id + "_24",
//                    name = "Bottome Right LocationType",
//                    value = "$none$",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "SpatialMeasure",
//                    id = location.id + "_18",
//                    name = "Top Left X Location",
//                    value = location.top_left_x,
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "numericValue",
//                });

//                values.Add(new Thing
//                {
//                    type = "SpatialMeasure",
//                    id = location.id + "_20",
//                    name = "Top Left Y Location",
//                    value = location.top_left_y,
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "numericValue"
//                });

//                values.Add(new Thing
//                {
//                    type = "SpatialMeasure",
//                    id = location.id + "_22",
//                    name = "Top Left Z Location",
//                    value = "0",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "numericValue"
//                });

//                values.Add(new Thing
//                {
//                    type = "SpatialMeasure",
//                    id = location.id + "_28",
//                    name = "Bottom Right X Location",
//                    value = location.bottom_right_x,
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "numericValue"
//                });

//                values.Add(new Thing
//                {
//                    type = "SpatialMeasure",
//                    id = location.id + "_30",
//                    name = "Bottom Right Y Location",
//                    value = location.bottom_right_y,
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "numericValue"
//                });

//                values.Add(new Thing
//                {
//                    type = "SpatialMeasure",
//                    id = location.id + "_32",
//                    name = "Bottom Right Z Location",
//                    value = "0",
//                    place1 = "$none$",
//                    place2 = "$none$",
//                    foundation = "IndividualType",
//                    value_type = "numericValue"
//                });

//                values5.AddRange(values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "describedBy",
//                    id = location.id + "_11",
//                    name = "$none$",
//                    value = "$none$",
//                    place1 = location.element_id,
//                    place2 = location.id + "_12",
//                    foundation = "namedBy",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "typeInstance",
//                    id = location.id + "_15",
//                    name = "$none$",
//                    value = "$none$",
//                    place1 = location.id + "_14",
//                    place2 = location.id + "_16",
//                    foundation = "typeInstance",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "typeInstance",
//                    id = location.id + "_25",
//                    name = "$none$",
//                    value = "$none$",
//                    place1 = location.id + "_24",
//                    place2 = location.id + "_26",
//                    foundation = "typeInstance",
//                    value_type = "$none$"
//                });


//                values.Add(new Thing
//                {
//                    type = "measureOfIndividualPoint",
//                    id = location.id + "_17",
//                    name = "$none$",
//                    value = "$none$",
//                    place1 = location.id + "_18",
//                    place2 = location.id + "_16",
//                    foundation = "typeInstance",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "measureOfIndividualPoint",
//                    id = location.id + "_19",
//                    name = "$none$",
//                    value = "$none$",
//                    place1 = location.id + "_20",
//                    place2 = location.id + "_16",
//                    foundation = "typeInstance",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "measureOfIndividualPoint",
//                    id = location.id + "_21",
//                    name = "$none$",
//                    value = "$none$",
//                    place1 = location.id + "_22",
//                    place2 = location.id + "_16",
//                    foundation = "typeInstance",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "measureOfIndividualPoint",
//                    id = location.id + "_27",
//                    name = "$none$",
//                    value = "$none$",
//                    place1 = location.id + "_28",
//                    place2 = location.id + "_26",
//                    foundation = "typeInstance",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "measureOfIndividualPoint",
//                    id = location.id + "_29",
//                    name = "$none$",
//                    value = "$none$",
//                    place1 = location.id + "_30",
//                    place2 = location.id + "_26",
//                    foundation = "typeInstance",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "measureOfIndividualPoint",
//                    id = location.id + "_31",
//                    name = "$none$",
//                    value = "$none$",
//                    place1 = location.id + "_32",
//                    place2 = location.id + "_26",
//                    foundation = "typeInstance",
//                    value_type = "$none$"
//                });

//                values7.AddRange(values);

//                values = new List<Thing>();

//                values.Add(new Thing
//                {
//                    type = "resourceInLocationType",
//                    id = location.id + "_13",
//                    name = "$none$",
//                    value = "$none$",
//                    place1 = location.id + "_12",
//                    place2 = location.id + "_14",
//                    foundation = "CoupleType",
//                    value_type = "$none$"
//                });

//                values.Add(new Thing
//                {
//                    type = "resourceInLocationType",
//                    id = location.id + "_23",
//                    name = "$none$",
//                    value = "$none$",
//                    place1 = location.id + "_12",
//                    place2 = location.id + "_24",
//                    foundation = "CoupleType",
//                    value_type = "$none$"
//                });


//                values6.AddRange(values);
//            }

//            locations = null;
//            values = null;
//            values2 = null;
//            values_dic = null;
//            values_dic2 = null;
//            results2 = null;

//            //Views

//            foreach (string[] current_lookup in View_Lookup)
//            {
//                sorted_results = new List<List<Thing>>();

//                results =
//                    from result in root.Elements("Class").Elements("MDDiagram").Elements("MDSymbol")
//                    where (string)result.Parent.Attribute("MDObjMinorTypeName") == current_lookup[1]
//                    where (string)result.Attribute("MDSymIdDef") != null
//                        || ((string)result.Attribute("MDObjMinorTypeName") == "Picture"
//                        || (string)result.Attribute("MDObjMinorTypeName") == "Doc Block")
//                    select new Thing
//                    {
//                        type = current_lookup[0],
//                        id = (string)result.Parent.Attribute("MDObjId") + (string)result.Attribute("MDSymIdDef"),
//                        name = ((string)result.Parent.Attribute("MDObjName")).Replace("&", " And "),
//                        place1 = (string)result.Parent.Attribute("MDObjId"),
//                        place2 = (string)result.Attribute("MDSymIdDef"),
//                        value = (string)result.Attribute("MDSymIdDef"),
//                        //value = Find_Def_DM2_Type((string)result.Attribute("MDSymIdDef"),ref values5),
//                        foundation = "$none$",
//                        value_type = "$element_type$"
//                    };
//                view_holder.Add(results.ToList());
//            }

//            root = null;

//            foreach (List<Thing> view_elements in view_holder)
//            {
//                //foreach (Thing thing in values)
//                int max = view_elements.Count;
//                for (int i = 0; i < max; i++)
//                {
//                    Thing thing = view_elements[i];
//                    //thing.value = (string) Find_Def_DM2_Type((string)thing.value, values5.ToList());
//                    if (thing.place2 != null)
//                    {
//                        if (things_dic.TryGetValue(thing.place2, out value))
//                            thing.value = (string)value.type;

//                        if (thing.type == "DIV-3")
//                        {
//                            values2 = new List<Thing>();
                            
//                            if (results_dic.TryGetValue(thing.place2, out values2))
//                            {
//                                foreach (Thing item in values2)
//                                {
//                                    view_elements.Add(new Thing
//                                    {
//                                        type = thing.type,
//                                        id = thing.id,
//                                        name = thing.name,
//                                        value = thing.value,
//                                        place1 = thing.place1,
//                                        place2 = item.place2,
//                                        foundation = thing.foundation,
//                                        value_type = thing.value_type
//                                    });
//                                    max++;
//                                }
//                            }
//                        }
//                    }
//                }

//                sorted_results = view_elements.GroupBy(x => x.place1).Select(group => group.Distinct().ToList()).ToList();

//                sorted_results_new = new List<List<Thing>>();
//                Add_Tuples(ref sorted_results, ref sorted_results_new, values3, ref errors_list);
//                Add_Tuples(ref sorted_results, ref sorted_results_new, values4, ref errors_list);
//                sorted_results = sorted_results_new;

//                foreach (List<Thing> view in sorted_results)
//                {

//                    if (view.Count() == 0)
//                        continue;

//                    mandatory_list = new List<Thing>();
//                    optional_list = new List<Thing>();

//                    if (view.First().type == "CV-1")
//                    {
//                        if (CV1_mandatory_views.TryGetValue(view.First().place1, out values))
//                            mandatory_list.AddRange(values);
//                    }

//                    if (view.First().type == "PV-1")
//                    {
//                        values = new List<Thing>();
//                        if (PV1_mandatory_views.TryGetValue(view.First().place1, out values))
//                            mandatory_list.AddRange(values);
//                    }

//                    foreach (Thing thing in view)
//                    {
//                        if (thing.place2 != null)
//                        {
//                            if (((string)thing.value).Substring(0, 1) != "_")
//                            {
//                                temp = Find_Mandatory_Optional((string)thing.value, view.First().name, thing.type, thing.place1, ref errors_list);
//                                if (temp == "Mandatory")
//                                {
//                                    mandatory_list.Add(new Thing { id = thing.place2, type = (string)thing.value, value = "$none$", value_type = "$none$" });
//                                }
//                                if (temp == "Optional")
//                                {
//                                    optional_list.Add(new Thing { id = thing.place2, type = (string)thing.value, value = "$none$", value_type = "$none$" });
//                                }
//                            }

//                            values = new List<Thing>();
//                            if (needline_mandatory_views.TryGetValue(thing.place2, out values))
//                            {
//                                if (Allowed_Needline(thing.type, values, ref things_dic) == true)
//                                {
//                                    mandatory_list.AddRange(values);
//                                    //if (!view.First().type.Contains("SV-4") && !view.First().type.Contains("SvcV-4") && !view.First().type.Contains("SV-10b"))
//                                    if (needline_optional_views.TryGetValue(thing.place2, out values2))
//                                        optional_list.AddRange(values2);
//                                        //optional_list.AddRange(needline_optional_views[thing.place2]);
//                                }
//                            }

//                            values = new List<Thing>();
//                            if (description_views.TryGetValue(thing.place2, out values))
//                                optional_list.AddRange(values);

//                            values = new List<Thing>();
//                            if (period_dic.TryGetValue(thing.place2, out values))
//                                optional_list.AddRange(values);

//                            values = new List<Thing>();
//                            if (datatype_mandatory_dic.TryGetValue(thing.place2, out values))
//                                mandatory_list.AddRange(values);

//                            values = new List<Thing>();
//                            if (datatype_optional_dic.TryGetValue(thing.place2, out values))
//                                optional_list.AddRange(values);

//                            if (thing.type.Contains("SvcV"))
//                            {
//                                if ((string)thing.value == "Service")
//                                {
//                                    values = new List<Thing>();

//                                    values.Add(new Thing
//                                    {
//                                        type = "ServiceDescription",
//                                        id = thing.place2 + "_2",
//                                        name = thing.place2 + "_Description",
//                                        value = "$none$",
//                                        place1 = "$none$",
//                                        place2 = "$none$",
//                                        foundation = "Individual",
//                                        value_type = "$none$"
//                                    });

//                                    values.Add(new Thing
//                                    {
//                                        type = "serviceDescribedBy",
//                                        id = thing.place2 + "_1",
//                                        name = "$none$",
//                                        value = "$none$",
//                                        place1 = thing.id,
//                                        place2 = thing.id + "_2",
//                                        foundation = "namedBy",
//                                        value_type = "$none$"
//                                    });

//                                    mandatory_list.AddRange(values);

//                                    values = new List<Thing>();

//                                    values.Add(new Thing
//                                    {
//                                        type = "ServiceDescription",
//                                        id = thing.place2 + "_2",
//                                        name = thing.place2 + "_Description",
//                                        value = "$none$",
//                                        place1 = "$none$",
//                                        place2 = "$none$",
//                                        foundation = "Individual",
//                                        value_type = "$none$"
//                                    });

//                                    values5.AddRange(values);

//                                    values = new List<Thing>();

//                                    values.Add(new Thing
//                                    {
//                                        type = "serviceDescribedBy",
//                                        id = thing.place2 + "_1",
//                                        name = "$none$",
//                                        value = "$none$",
//                                        place1 = thing.id,
//                                        place2 = thing.id + "_2",
//                                        foundation = "namedBy",
//                                        value_type = "$none$"
//                                    });

//                                    values7.AddRange(values);
//                                }
//                            }
//                            else if (thing.type == "OV-6c")
//                            {
//                                values = new List<Thing>();
//                                if (OV6c_aro_optional_views.TryGetValue(thing.place2, out values))
//                                {
//                                    optional_list.AddRange(values);
//                                }
//                            }
//                            else if (thing.type == "OV-5b" || thing.type == "OV-6b")
//                            {
//                                values = new List<Thing>();
//                                if (OV5b_aro_optional_views.TryGetValue(thing.place2, out values))
//                                {
//                                    optional_list.AddRange(values);
//                                    mandatory_list.AddRange(OV5b_aro_mandatory_views[thing.place2]);
//                                }
//                            }
//                            else if (thing.type == "DIV-3")
//                            {
//                                values = new List<Thing>();
//                                if (DIV3_optional.TryGetValue(thing.place2, out values))
//                                {
//                                    optional_list.AddRange(values);
//                                }
//                                values = new List<Thing>();
//                                if (DIV3_mandatory.TryGetValue(thing.place2, out values))
//                                {
//                                    mandatory_list.AddRange(values);
//                                }
//                            }
//                            else if (thing.type == "OV-4")
//                            {
//                                values = new List<Thing>();
//                                if (OV4_support_optional_views.TryGetValue(thing.place2, out values))
//                                    if (Allowed_Class("OV-4",(string)thing.value))
//                                        optional_list.AddRange(values);
//                            }
//                            else if (thing.type == "OV-2")
//                            {
//                                values = new List<Thing>();
//                                if (OV2_support_mandatory_views.TryGetValue(thing.place2, out values))
//                                    if (Allowed_Class("OV-2", (string)thing.value))
//                                        mandatory_list.AddRange(values);
//                                values = new List<Thing>();
//                                if (OV2_support_optional_views.TryGetValue(thing.place2, out values))
//                                    if (Allowed_Class("OV-2", (string)thing.value))
//                                        optional_list.AddRange(values);
//                            }
//                            else if (thing.type == "AV-1" || thing.type.Contains("PV"))
//                            {
//                                if ((string)thing.value == "ProjectType")
//                                {
//                                    values = new List<Thing>();

//                                    values.Add(new Thing
//                                    {
//                                        type = "typeInstance",
//                                        id = thing.place2 + "_1",
//                                        name = "$none$",
//                                        value = "$none$",
//                                        place1 = thing.id,
//                                        place2 = thing.id + "_2",
//                                        foundation = "typeInstance",
//                                        value_type = "$none$"
//                                    });

//                                    optional_list.AddRange(values);

//                                    values = new List<Thing>();

//                                    values.Add(new Thing
//                                    {
//                                        type = "Project",
//                                        id = thing.place2 + "_2",
//                                        name = thing.place2 + "_Project",
//                                        value = "$none$",
//                                        place1 = "$none$",
//                                        place2 = "$none$",
//                                        foundation = "Individual",
//                                        value_type = "$none$"
//                                    });

//                                    mandatory_list.AddRange(values);

//                                    values = new List<Thing>();

//                                    values.Add(new Thing
//                                    {
//                                        type = "typeInstance",
//                                        id = thing.place2 + "_1",
//                                        name = "$none$",
//                                        value = "$none$",
//                                        place1 = thing.id,
//                                        place2 = thing.id + "_2",
//                                        foundation = "typeInstance",
//                                        value_type = "$none$"
//                                    });

//                                    values7.AddRange(values);

//                                    values = new List<Thing>();

//                                    values.Add(new Thing
//                                    {
//                                        type = "Project",
//                                        id = thing.place2 + "_2",
//                                        name = thing.place2 + "_Project",
//                                        value = "$none$",
//                                        place1 = "$none$",
//                                        place2 = "$none$",
//                                        foundation = "Individual",
//                                        value_type = "$none$"
//                                    });

//                                    values5.AddRange(values);
//                                }
//                                if (thing.type == "PV-1")
//                                {
//                                    if ((string)thing.value == "OrganizationType")
//                                    {
//                                        values = new List<Thing>();

//                                        values.Add(new Thing
//                                        {
//                                            type = "typeInstance",
//                                            id = thing.place2 + "_1",
//                                            name = "$none$",
//                                            value = "$none$",
//                                            place1 = thing.id,
//                                            place2 = thing.id + "_2",
//                                            foundation = "typeInstance",
//                                            value_type = "$none$"
//                                        });

//                                        optional_list.AddRange(values);

//                                        values = new List<Thing>();

//                                        values.Add(new Thing
//                                        {
//                                            type = "Organization",
//                                            id = thing.place2 + "_2",
//                                            name = thing.place2 + "_Organization",
//                                            value = "$none$",
//                                            place1 = "$none$",
//                                            place2 = "$none$",
//                                            foundation = "Individual",
//                                            value_type = "$none$"
//                                        });

//                                        mandatory_list.AddRange(values);

//                                        values = new List<Thing>();

//                                        values.Add(new Thing
//                                        {
//                                            type = "typeInstance",
//                                            id = thing.place2 + "_1",
//                                            name = "$none$",
//                                            value = "$none$",
//                                            place1 = thing.id,
//                                            place2 = thing.id + "_2",
//                                            foundation = "typeInstance",
//                                            value_type = "$none$"
//                                        });

//                                        values7.AddRange(values);

//                                        values = new List<Thing>();

//                                        values.Add(new Thing
//                                        {
//                                            type = "Organization",
//                                            id = thing.place2 + "_2",
//                                            name = thing.place2 + "_Organization",
//                                            value = "$none$",
//                                            place1 = "$none$",
//                                            place2 = "$none$",
//                                            foundation = "Individual",
//                                            value_type = "$none$"
//                                        });

//                                        values5.AddRange(values);
//                                    }
//                                }
//                            }
//                            else if (thing.type == "CV-1")
//                            {
//                                values = new List<Thing>();
//                                if (CV1_mandatory_views.TryGetValue(thing.place2, out values))
//                                    mandatory_list.AddRange(values);
//                                values = new List<Thing>();
//                                if (CV1_optional_views.TryGetValue(thing.place2, out values))
//                                    optional_list.AddRange(values);
//                            }
//                            else if (thing.type == "CV-4")
//                            {
//                                values = new List<Thing>();
//                                if (CV4_mandatory_views.TryGetValue(thing.place2, out values))
//                                    mandatory_list.AddRange(values);
//                                values = new List<Thing>();
//                                if (CV4_optional_views.TryGetValue(thing.place2, out values))
//                                    optional_list.AddRange(values);
//                            }
                          
//                        }
//                    }

//                    mandatory_list = mandatory_list.GroupBy(x => x.id).Select(grp => grp.First()).ToList();
//                    optional_list = optional_list.GroupBy(x => x.id).Select(grp => grp.First()).ToList();

//                    values = new List<Thing>();
//                    if(doc_blocks_views.TryGetValue(view.First().place1, out values))
//                        optional_list.AddRange(values);

//                    values = new List<Thing>();
//                    if (OV1_pic_views.TryGetValue(view.First().place1, out values))
//                            mandatory_list.AddRange(values);

//                    mandatory_list = mandatory_list.OrderBy(x => x.type).ToList();
//                    optional_list = optional_list.OrderBy(x => x.type).ToList();

//                    if (Proper_View(mandatory_list, view.First().name,view.First().type, view.First().place1, ref errors_list))
//                        views.Add(new View { type = view.First().type, id = view.First().place1, name = view.First().name, mandatory = mandatory_list, optional = optional_list });
//                    //else
//                    //{
//                    //    test = false;
//                    //}
//                }
//            }

//            results_dic = null;
//            mandatory_list = null;
//            optional_list = null;
//            view_holder = null;

//            using (var sw = new Utf8StringWriter())
//            {
//                using (var writer = XmlWriter.Create(sw))
//                {

//                    writer.WriteRaw(@"<IdeasEnvelope OriginatingNationISO3166TwoLetterCode=""String"" ism:ownerProducer=""NMTOKEN"" ism:classification=""U""
//                    xsi:schemaLocation=""http://cio.defense.gov/xsd/dm2 DM2_PES_v2.02_Chg_1.XSD""
//                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:ism=""urn:us:gov:ic:ism:v2"" xmlns:ideas=""http://www.ideasgroup.org/xsd""
//                    xmlns:dm2=""http://www.ideasgroup.org/dm2""><IdeasData XMLTagsBoundToNamingScheme=""DM2Names"" ontologyVersion=""2.02_Chg_1"" ontology=""DM2"">
//		            <NamingScheme ideas:FoundationCategory=""NamingScheme"" id=""ns1""><ideas:Name namingScheme=""ns1"" id=""NamingScheme"" exemplarText=""DM2Names""/>
//		            </NamingScheme>");
//                    writer.WriteRaw("\n");
//                    if (representation_scheme)
//                    {
//                        writer.WriteRaw(@"<RepresentationScheme ideas:FoundationCategory=""Type"" id=""id_rs1"">
//			            <ideas:Name id=""RepresentationScheme"" namingScheme=""ns1"" exemplarText=""Base64 Encoded Image""/>
//		                </RepresentationScheme>");
//                        writer.WriteRaw("\n");
//                    }

//                    values = things_dic.Select(kvp => kvp.Value).ToList();
//                    things_dic = null;
//                    foreach (Thing thing in values)
//                        writer.WriteRaw("<" + thing.type + " ideas:FoundationCategory=\"" + thing.foundation + "\" id=\"id" + thing.id + "\" "
//                            + (((string)thing.value_type).Contains("$") ? "" : thing.value_type + "=\"" + (string)thing.value + "\"") + ">" + "<ideas:Name exemplarText=\"" + thing.name
//                            + "\" namingScheme=\"ns1\" id=\"n" + thing.id + "\"/></" + thing.type + ">\n");
//                    values = null;

//                    foreach (Thing thing in values5)
//                        writer.WriteRaw("<" + thing.type + " ideas:FoundationCategory=\"" + thing.foundation + "\" id=\"id" + thing.id + "\" "
//                            + (((string)thing.value_type).Contains("$") ? "" : thing.value_type + "=\"" + (string)thing.value + "\"") + ">" + "<ideas:Name exemplarText=\"" + thing.name
//                            + "\" namingScheme=\"ns1\" id=\"n" + thing.id + "\"/></" + thing.type + ">\n");
//                    values5 = null;

//                    foreach (Thing thing in values4)
//                        writer.WriteRaw("<" + thing.type + " ideas:FoundationCategory=\"" + thing.foundation + "\" id=\"id" + thing.id
//                        + "\" place1Type=\"id" + thing.place1 + "\" place2Type=\"id" + thing.place2 + "\""
//                        + (((string)thing.name).Contains("$") ? "/>\n" : ">" + "<ideas:Name exemplarText=\"" + thing.name
//                        + "\" namingScheme=\"ns1\" id=\"n" + thing.id + "\"/></" + thing.type + ">\n"));
//                    values4 = null;

//                    foreach (Thing thing in values6)
//                        writer.WriteRaw("<" + thing.type + " ideas:FoundationCategory=\"" + thing.foundation + "\" id=\"id" + thing.id
//                        + "\" place1Type=\"id" + thing.place1 + "\" place2Type=\"id" + thing.place2 + "\""
//                        + (((string)thing.name).Contains("$") ? "/>\n" : ">" + "<ideas:Name exemplarText=\"" + thing.name
//                        + "\" namingScheme=\"ns1\" id=\"n" + thing.id + "\"/></" + thing.type + ">\n"));
//                    values6 = null;

//                    foreach (Thing thing in values3)
//                        writer.WriteRaw("<" + thing.type + " ideas:FoundationCategory=\"" + thing.foundation + "\" id=\"id" + thing.id
//                        + "\" tuplePlace1=\"id" + thing.place1 + "\" tuplePlace2=\"id" + thing.place2 + "\""
//                        + (((string)thing.name).Contains("$") ? "/>\n" : ">" + "<ideas:Name exemplarText=\"" + thing.name
//                        + "\" namingScheme=\"ns1\" id=\"n" + thing.id + "\"/></" + thing.type + ">\n"));
//                    values3 = null;

//                    foreach (Thing thing in values7)
//                        writer.WriteRaw("<" + thing.type + " ideas:FoundationCategory=\"" + thing.foundation + "\" id=\"id" + thing.id
//                        + "\" tuplePlace1=\"id" + thing.place1 + "\" tuplePlace2=\"id" + thing.place2 + "\""
//                        + (((string)thing.name).Contains("$") ? "/>\n" : ">" + "<ideas:Name exemplarText=\"" + thing.name
//                        + "\" namingScheme=\"ns1\" id=\"n" + thing.id + "\"/></" + thing.type + ">\n"));
//                    values7 = null;

//                    writer.WriteRaw("</IdeasData>\n");

//                    writer.WriteRaw("<IdeasViews frameworkVersion=\"DM2.02_Chg_1\" framework=\"DoDAF\">\n");

//                    foreach (View view in views)
//                    {
//                        writer.WriteRaw("<" + view.type + " id=\"id" + view.id + "\" name=\"" + view.name + "\">\n");

//                        writer.WriteRaw("<MandatoryElements>\n");

//                        foreach (Thing thing in view.mandatory)
//                        {
//                            writer.WriteRaw("<" + view.type + "_" + thing.type + " ref=\"id" + thing.id + "\"/>\n");
//                        }

//                        writer.WriteRaw("</MandatoryElements>\n");
//                        writer.WriteRaw("<OptionalElements>\n");

//                        foreach (Thing thing in view.optional)
//                        {
//                            writer.WriteRaw("<" + view.type + "_" + thing.type + " ref=\"id" + thing.id + "\"/>\n");
//                        }

//                        writer.WriteRaw("</OptionalElements>\n");
//                        writer.WriteRaw("</" + view.type + ">\n");
//                    }

//                    views = null;

//                    writer.WriteRaw("</IdeasViews>\n");

//                    writer.WriteRaw("</IdeasEnvelope>\n");

//                    writer.Flush();
//                }

//                output = sw.ToString();
//                errors = string.Join("", errors_list.Distinct().ToArray());

//                if (errors.Count() > 0)
//                    test = false;

//                return test;
//            }
//        }

        ////////////////////
        ////////////////////

        public static bool MD2PES(byte[] input, ref string output, ref string errors)
        {
            IEnumerable<Location> locations = new List<Location>();
            IEnumerable<Thing> things = new List<Thing>();
            IEnumerable<Thing> tuple_types = new List<Thing>();
            IEnumerable<Thing> tuples = new List<Thing>();
            IEnumerable<Thing> results;
            IEnumerable<Location> results_loc;
            List<View> views = new List<View>();
            List<Thing> mandatory_list = new List<Thing>();
            List<Thing> optional_list = new List<Thing>();
            string temp;
            Dictionary<string, List<Thing>> needline_mandatory_views = new Dictionary<string, List<Thing>>();
            Dictionary<string, List<Thing>> needline_optional_views = new Dictionary<string, List<Thing>>();
            Dictionary<string, Thing> things_dic = new Dictionary<string, Thing>();
            Dictionary<string, List<Thing>> lookup = new Dictionary<string, List<Thing>>();
            Dictionary<string, List<Thing>> div2_dic = new Dictionary<string, List<Thing>>();
            Dictionary<string, List<Thing>> div2_dic2 = new Dictionary<string, List<Thing>>();
            Dictionary<string, List<Thing>> div3_dic = new Dictionary<string, List<Thing>>();
            Dictionary<string, List<Thing>> div3_dic2 = new Dictionary<string, List<Thing>>();
            Dictionary<string, List<Thing>>  results_dic = new Dictionary<string, List<Thing>>();
            Dictionary<string, List<Thing>>  results_dic2 = new Dictionary<string, List<Thing>>();
            Dictionary<string, List<Thing>>  results_dic3 = new Dictionary<string, List<Thing>>();
            Dictionary<string, List<Thing>> aro;
            Dictionary<string, List<Thing>> aro2;
            Dictionary<string, Thing> values_dic;
            Dictionary<string, Thing> values_dic2;
            Dictionary<string, Thing> values_dic3;
            Dictionary<string, Thing> values_dic4;
            Dictionary<string, List<Thing>> description_views = new Dictionary<string, List<Thing>>();
            XElement root = XElement.Load(new MemoryStream(input));
            List<List<Thing>> sorted_results = new List<List<Thing>>();
            List<List<Thing>> sorted_results_new = new List<List<Thing>>();
            bool representation_scheme = false;
            List<Thing> values = new List<Thing>();
            List<Thing> values2 = new List<Thing>();
            XNamespace ns = "http://www.omg.org/spec/UPDM/20121004/UPDM-Profile";
            XNamespace ns2 = "http://www.omg.org/spec/XMI/20131001";
            XNamespace ns3 = "http://www.omg.org/spec/UML/20131001";
            XNamespace ns4 = "http://www.magicdraw.com/schemas/Events.xmi";
            Thing value;
            Thing value2;
            List<Thing> values3 = new List<Thing>();
            List<Thing> values4 = new List<Thing>();
            List<Thing> values5 = new List<Thing>();
            List<Thing> values6 = new List<Thing>();
            List<Thing> values7 = new List<Thing>();
            List<string> errors_list = new List<string>();
            bool test = true;
            List<List<Thing>> view_holder = new List<List<Thing>>();
            Dictionary<string, List<Thing>> OV1_pic_views = new Dictionary<string, List<Thing>>();
            int count=0;
            int count2 = 0;
            bool add = false;

            //Regular Things

            foreach (string[] current_lookup in MD_Element_Lookup)
            {

                results =
                    //from result3 in root.Elements(ns + "View")
                    //from result4 in root.Descendants()
                    from result2 in root.Descendants()
                    //from result2 in root.Elements(ns3 + "Package").Elements("packagedElement")
                    //where result2.Attribute("name") != null
                    where (string)result2.Attribute("name") != null
                    from result in root.Elements(ns + current_lookup[1])
                    where (string)result.Attribute(current_lookup[3]) == (string)result2.Attribute(ns2 + "id")
                    //where (string)result3.LastAttribute == (string)result4.Attribute(ns2 + "id")
                    select new Thing
                    {
                        type = current_lookup[0],
                        id = (string)result2.Attribute(ns2 + "id"),// + "///" +*/ (string)result.LastAttribute,//Attribute("base_Operation"),
                        name = (string)result2.Attribute("name"),//*/ (string)result.FirstAttribute,//Attribute(ns2 + "id"),
                        value = "$none$",
                        //place1 = (string)result.Attribute(ns2 + "id"),
                        //place2 = (string)result.LastAttribute,
                        foundation = current_lookup[2],
                        value_type = "$none$"
                    };

                things = things.Concat(results.ToList());

                //things = things.GroupBy(x => x.id).Select(grp => grp.First());

                //Regular Descriptions

                results_dic =
                            (
                             from result2 in root.Descendants().Elements("ownedComment")
                             where (string)result2.Parent.Attribute(ns2 + "id") != null
                             where (string)result2.Parent.Attribute("name") != null
                             from result in root.Elements(ns + current_lookup[1])
                             where (string)result.Attribute(current_lookup[3]) == (string)result2.Parent.Attribute(ns2 + "id")
                             select new
                             {
                                 key = (string)result.Attribute(current_lookup[3]),
                                 value = new List<Thing> {
                                    new Thing
                                    {
                                        type = "Information",
                                        id = (string)result.Attribute(current_lookup[3]) + "_9",
                                        name = ((string)result2.Parent.Attribute("name")).Replace("&", " And ").Replace("<", "").Replace(">", "") + " Description",
                                        value = ((((((string)result2.Attribute("body")).Replace("@", " At ")).Replace("\"","'")).Replace("&", " And ")).Replace("<", "")).Replace(">", ""),
                                        place1 = (string)result.Attribute(current_lookup[3]),
                                        place2 = (string)result.Attribute(current_lookup[3]) + "_9",
                                        foundation = "IndividualType",
                                        value_type = "exemplar"
                                    }
                                }
                             }).ToDictionary(a => a.key, a => a.value);

                things = things.Concat(results_dic.SelectMany(x => x.Value));

                foreach (Thing thing in results_dic.SelectMany(x => x.Value))
                {

                    value = new Thing
                    {
                        type = "describedBy",
                        id = thing.place1 + "_10",
                        foundation = "namedBy",
                        place1 = thing.place1,
                        place2 = thing.place2,
                        name = "$none$",
                        value = "$none$",
                        value_type = "$none$"
                    };
                    tuples = tuples.Concat(new List<Thing> { value });
                    description_views.Add(thing.place1, new List<Thing> { value });
                }

                MergeDictionaries(description_views, results_dic);
            
            }

            //Events

            results =
               from result in root.Elements(ns4 + "NoneStartEvent")
               where (string)result.Attribute("id") != null
               from result2 in root.Descendants().Elements("edge")
               where (string)result2.Attribute("source") == (string)result.Attribute("base_InitialNode")
               from result3 in root.Descendants().Elements("node")
               where (string)result3.Attribute("behavior") != null
               where (string)result3.Attribute(ns2+"id") == (string)result2.Attribute("target")

               //where (string)result3.LastAttribute == (string)result4.Attribute(ns2 + "id")
               select new Thing
               {
                   type = "Activity",
                   id = (string)result.Attribute("base_InitialNode"),
                   name = (string)result.Attribute("id"),//*/ (string)result.FirstAttribute,//Attribute(ns2 + "id"),
                   value = "$none$",
                   place1 = (string)result3.Attribute("behavior"),
                   //place2 = (string)result.LastAttribute,
                   foundation = "IndividualType",
                   value_type = "$none$"
               };

            things = things.Concat(results.ToList());

            values = new List<Thing>();

            foreach (Thing thing in results)
            {
                values.Add(new Thing
                    {
                        type = "BeforeAfterType",
                        id = thing.id + thing.place1+"_1",
                        name = "$none$",
                        value = (string)thing.id + thing.place1 + "_1",
                        place1 = thing.id,
                        place2 = thing.place1,
                        foundation = "CoupleType",
                        value_type = "$id$"
                    });    
            }

            tuple_types = tuple_types.Concat(values);

            var duplicateKeys1 = things.GroupBy(x => x.id)
                       .Where(group => group.Count() > 1)
                       .Select(group => group.Key);

            results =
                 from result in root.Elements(ns4 + "NoneEndEvent")
                 where (string)result.Attribute("id") != null
                 from result2 in root.Descendants().Elements("edge")
                 where (string)result2.Attribute("target") == (string)result.Attribute("base_ActivityFinalNode")
                 from result3 in root.Descendants().Elements("node")
                 where (string)result3.Attribute("behavior") != null
                 where (string)result3.Attribute(ns2+"id") == (string)result2.Attribute("source")

                 select new Thing
                 {
                     type = "Activity",
                     id = (string)result.Attribute("base_ActivityFinalNode"),
                     name = (string)result.Attribute("id"),//*/ (string)result.FirstAttribute,//Attribute(ns2 + "id"),
                     value = "$none$",
                     place1 = (string)result3.Attribute("behavior"),
                     //place2 = (string)result.LastAttribute,
                     foundation = "IndividualType",
                     value_type = "$none$"
                 };

            things = things.Concat(results.ToList());

            values = new List<Thing>();

            foreach (Thing thing in results)
            {
                values.Add(new Thing
                {
                    type = "BeforeAfterType",
                    id = thing.place1 + thing.id + "_1",
                    name = "$none$",
                    value = (string)thing.place1 + thing.id + "_1",
                    place1 = thing.place1,
                    place2 = thing.id,
                    foundation = "CoupleType",
                    value_type = "$id$"
                });
            }

            tuple_types = tuple_types.Concat(values);

            var duplicateKeys2 = things.GroupBy(x => x.id)
                       .Where(group => group.Count() > 1)
                       .Select(group => group.Key);

            //Regular Relationships

            foreach (string[] current_lookup in MD_Relationship_Lookup)
            {

                if (current_lookup[2] == "1")
                {

                    results =
                        from result in root.Elements(ns + current_lookup[1])
                        //from result3 in root.Elements(ns + "View")
                        //from result4 in root.Descendants()
                        from result2 in root.Descendants()
                        //from result2 in root.Elements(ns3 + "Package").Elements("packagedElement")
                        //where result2.Attribute("name") != null
                        where (string)result.LastAttribute == (string)result2.Attribute(ns2 + "id")
                        //where (string)result3.LastAttribute == (string)result4.Attribute(ns2 + "id")
                        select new Thing
                        {
                            type = current_lookup[0],
                            id = (string)result.LastAttribute,
                            name = "$none$",
                            value = (string)result2.Element("supplier").Attribute(ns2 + "idref") + (string)result2.Element("client").Attribute(ns2 + "idref"),
                            place2 = (string)result2.Element("client").Attribute(ns2 + "idref"),
                            place1 = (string)result2.Element("supplier").Attribute(ns2 + "idref"),
                            foundation = current_lookup[2],
                            value_type = "$id$"
                        };

                    tuple_types = tuple_types.Concat(results.ToList());
                }
                else
                {
                    results =
                        from result in root.Elements(ns + current_lookup[1])
                        //from result3 in root.Elements(ns + "View")
                        //from result4 in root.Descendants()
                        from result2 in root.Descendants()
                        //from result2 in root.Elements(ns3 + "Package").Elements("packagedElement")
                        //where result2.Attribute("name") != null
                        where (string)result.LastAttribute == (string)result2.Attribute(ns2 + "id")
                        //where (string)result3.LastAttribute == (string)result4.Attribute(ns2 + "id")
                        select new Thing
                        {
                            type = current_lookup[0],
                            id = (string)result.LastAttribute,
                            name = "$none$",
                            value = (string)result2.Element("client").Attribute(ns2 + "idref") + (string)result2.Element("supplier").Attribute(ns2 + "idref"),
                            place1 = (string)result2.Element("client").Attribute(ns2 + "idref"),
                            place2 = (string)result2.Element("supplier").Attribute(ns2 + "idref"),
                            foundation = current_lookup[2],
                            value_type = "$id$"
                        };

                    tuple_types = tuple_types.Concat(results.ToList());
                }

            }

            //OV-6c BPMN

            results =
                from result in root.Descendants().Elements("node")
                where (string)result.Parent.Attribute("represents") != null
                from result2 in root.Descendants().Elements("node")
                where (string)result2.Attribute("behavior") != null
                where (string)result.Attribute(ns2 + "idref") == (string)result2.Attribute(ns2 + "id")
                //from result3 in root.Descendants().Elements("covered")
                //where (string)result3.Attribute(ns2 + "idref") == (string)result.Attribute(ns2 + "id")
                //where (string)result3.Parent.Attribute("message") != null

                select
                    //new {
                    //    key = (string)result.Parent.Parent.Attribute("SAObjId"),
                    //    value = new List<Thing> {
                           new Thing

                           {
                               type = "activityPerformedByPerformer",
                               id = (string)result2.Attribute("behavior") + (string)result.Parent.Attribute("represents"),
                               name = "$none$",
                               value = (string)result.Parent.Attribute("represents") + (string)result2.Attribute("behavior") ,
                               place2 = (string)result2.Attribute("behavior"),
                               place1 = (string)result.Parent.Attribute("represents"),
                               foundation = "CoupleType",
                               value_type = "$id$"

                           };

            tuple_types = tuple_types.Concat(results);

            //OV-6c ETD

            results =
                   from result in root.Descendants().Elements("fragment")
                   from result2 in root.Descendants().Elements("message")
                   where (string)result2.Attribute("name") != null
                   where (string)result.Attribute("message") == (string)result2.Attribute(ns2 + "id")

                   select new Thing
                   {
                       type = "Activity",
                       id = (string)result.Attribute(ns2 + "id"),// + "///" +*/ (string)result.LastAttribute,//Attribute("base_Operation"),
                       name = (string)result2.Attribute("name"),//*/ (string)result.FirstAttribute,//Attribute(ns2 + "id"),
                       value = "$none$",
                       //place1 = (string)result.Attribute(ns2 + "id"),
                       //place2 = (string)result.LastAttribute,
                       foundation = "IndividualType",
                       value_type = "$none$"
                   };

            things = things.Concat(results.ToList());

            results =
                from result in root.Descendants().Elements("coveredBy")
                from result2 in root.Descendants().Elements("ownedAttribute")
                where (string)result2.Attribute("type") != null
                where (string)result.Parent.Attribute("represents") == (string)result2.Attribute(ns2 + "id")
                
                //from result3 in root.Descendants().Elements("covered")
                //where (string)result3.Attribute(ns2 + "idref") == (string)result.Attribute(ns2 + "id")
                //where (string)result3.Parent.Attribute("message") != null

                select
                    //new {
                    //    key = (string)result.Parent.Parent.Attribute("SAObjId"),
                    //    value = new List<Thing> {
                           new Thing

                           {
                               type = "activityPerformedByPerformer",
                               id = (string)result2.Attribute("type") + (string)result.Attribute(ns2 + "idref"),
                               name = "$none$",
                               value = (string)result2.Attribute("type") + (string)result.Attribute(ns2 + "idref"),
                               place1 = (string)result2.Attribute("type"),
                               place2 = (string)result.Attribute(ns2 + "idref"),
                               foundation = "CoupleType",
                               value_type = "$id$"

                           };

            tuple_types = tuple_types.Concat(results);

            tuple_types = tuple_types.GroupBy(x => (string)x.value).Select(grp => grp.First());

            results =
                from result in root.Descendants().Elements("message")
                //from result2 in root.Descendants().Elements("fragment")
                //where (string)result.Attribute("sendEvent") == (string)result2.Attribute(ns2 + "id")
                //from result3 in root.Descendants().Elements("fragment")
                //where (string)result.Attribute("receiveEvent") == (string)result3.Attribute(ns2 + "id")

                select
                    //new {
                    //    key = (string)result.Parent.Parent.Attribute("SAObjId"),
                    //    value = new List<Thing> {
               new Thing

               {
                   type = "BeforeAfterType",
                   id = (string)result.Attribute("sendEvent") + (string)result.Attribute("receiveEvent") + "_1",
                   name = "$none$",
                   value = "$none$",
                   place1 = (string)result.Attribute("sendEvent"),
                   place2 = (string)result.Attribute("receiveEvent"),
                   foundation = "CoupleType",
                   value_type = "$period$"

               };

            tuple_types = tuple_types.Concat(results);

            //DIV-3 Data Type Parts

            results =
                    from result in root.Elements(ns + "EntityItem")
                    //from result3 in root.Elements(ns + "View")
                    //from result4 in root.Descendants()
                    from result2 in root.Descendants().Elements("ownedAttribute")
                    //from result2 in root.Elements(ns3 + "Package").Elements("packagedElement")
                    //where result2.Attribute("name") != null
                    where (string)result.Attribute("base_Class") == (string)result2.Parent.Attribute(ns2 + "id")
                    //where (string)result3.LastAttribute == (string)result4.Attribute(ns2 + "id")
                    from result3 in root.Descendants().Elements("referenceExtension")
                    where (string)result3.Parent.Parent.Parent.Attribute(ns2 + "id") == (string)result2.Attribute(ns2 + "id")
                    select new Thing
                    {
                        type = "DataType",
                        id = (string)result3.Attribute("originalID") + (string)result2.Attribute(ns2 + "id"),// + "///" +*/ (string)result.LastAttribute,//Attribute("base_Operation"),
                        name = (string)result3.Attribute("referentPath"),//*/ (string)result.FirstAttribute,//Attribute(ns2 + "id"),
                        value = "$none$",
                        place1 = (string)result3.Attribute("originalID") + (string)result2.Attribute(ns2 + "id"),
                        place2 = (string)result2.Parent.Attribute(ns2 + "id"),
                        foundation = "Individual_Type",
                        value_type = "$none$"
                    };

            things = things.Concat(results.ToList());
            div3_dic = results.GroupBy(x => x.place2).ToDictionary(gdc => gdc.Key, gdc => gdc.ToList());

            foreach (Thing thing in results)
            {
                value = new Thing
                {
                    type = "typeInstance",
                    id = thing.place1 + thing.place2 + "ti1",
                    name = "$none$",
                    value = "$none$",
                    place1 = thing.place2,
                    place2 = thing.place1,
                    foundation = "typeInstance",
                    value_type = "$none$"
                };
                tuples = tuples.Concat(new List<Thing>() { value }
                );
                values = new List<Thing>();
                if (div3_dic2.TryGetValue(thing.place2, out values))
                {
                    values.Add(value);
                    div3_dic2.Remove(thing.place2);
                    div3_dic2.Add(thing.place2, values);
                }
                else
                    div3_dic2.Add(thing.place2, new List<Thing>() { value });

            }

            //DIV Data Parts

            results =
                    from result in root.Elements(ns + "EntityItem")
                    //from result3 in root.Elements(ns + "View")
                    //from result4 in root.Descendants()
                    from result2 in root.Descendants().Elements("ownedAttribute")
                    //where (string)result2.Attribute("aggregation") == "composite"
                    //from result2 in root.Elements(ns3 + "Package").Elements("packagedElement")
                    //where result2.Attribute("name") != null
                    where (string)result.Attribute("base_Class") == (string)result2.Parent.Attribute(ns2 + "id")
                    //where (string)result3.LastAttribute == (string)result4.Attribute(ns2 + "id")
                    select new Thing
                    {
                        type = "Data",
                        id = (string)result2.Attribute(ns2 + "id"),// + "///" +*/ (string)result.LastAttribute,//Attribute("base_Operation"),
                        name = (string)result2.Attribute("name"),//*/ (string)result.FirstAttribute,//Attribute(ns2 + "id"),
                        value = "$none$",
                        place1 = (string)result2.Attribute(ns2 + "id"),
                        place2 = (string)result.Attribute("base_Class"),
                        foundation = "Individual_Type",
                        value_type = "$none$"
                    };

            //things = things.Concat(results.ToList());
            div2_dic = results.GroupBy(x => x.place2).ToDictionary(gdc => gdc.Key, gdc => gdc.ToList());

            foreach (Thing thing in results)
            {
                value = new Thing
                {
                    type = "WholePartType",
                    id = thing.place1 + thing.place2,
                    name = "$none$",
                    value = "$none$",
                    place1 = thing.place2,
                    place2 = thing.place1,
                    foundation = "WholePartType",
                    value_type = "$none$"
                };
                tuple_types = tuple_types.Concat(new List<Thing>() { value }
                );
                values = new List<Thing>();
                if (div2_dic2.TryGetValue(thing.place2, out values))
                {
                    values.Add(value);
                    div2_dic2.Remove(thing.place2);
                    div2_dic2.Add(thing.place2, values);
                }
                else
                    div2_dic2.Add(thing.place2, new List<Thing>() { value });

            }

            //SuperSubtupe

            results =
                from result in root.Descendants().Elements("generalization")
                //from result3 in root.Elements(ns + "View")
                //from result4 in root.Descendants()
                //from result2 in root.Descendants()
                //from result2 in root.Elements(ns3 + "Package").Elements("packagedElement")
                //where result2.Attribute("name") != null
                //where (string)result.Attribute == (string)result2.Attribute(ns2 + "id")
                //where (string)result3.LastAttribute == (string)result4.Attribute(ns2 + "id")
                where (string)result.Attribute("general") != null
                where (string)result.Parent.Attribute(ns2 + "id") != null
                select new Thing
                {
                    type = "superSubtype",
                    id = (string)result.Attribute("general") + (string)result.Parent.Attribute(ns2 + "id"),
                    name = "$none$",
                    value = "$none$",
                    place1 = (string)result.Attribute("general"),
                    place2 = (string)result.Parent.Attribute(ns2 + "id"),
                    foundation = "superSubtype",
                    value_type = "$none$"
                };

            tuples = tuples.Concat(results.ToList());

            //WholePartType

            results =
                from result in root.Descendants().Elements("ownedAttribute")
                //from result3 in root.Elements(ns + "View")
                //from result4 in root.Descendants()
                //from result2 in root.Descendants()
                //from result2 in root.Elements(ns3 + "Package").Elements("packagedElement")
                where (string)result.Attribute("aggregation") == "composite" || (string)result.Attribute("aggregation") == "shared"
                //where (string)result.Attribute == (string)result2.Attribute(ns2 + "id")
                //where (string)result3.LastAttribute == (string)result4.Attribute(ns2 + "id")
                where (string)result.Parent.Attribute(ns2 + "id") != null
                where (string)result.Attribute("type") != null
                select new Thing
                {
                    type = "WholePartType",
                    id = (string)result.Parent.Attribute(ns2 + "id") + (string)result.Attribute("type"),
                    name = "$none$",
                    value = "$none$",
                    place1 = (string)result.Attribute("type"),
                    place2 = (string)result.Parent.Attribute(ns2 + "id"),
                    foundation = "WholePartType",
                    value_type = "$none$"
                };

            tuple_types = tuple_types.Concat(results.ToList());

            //WholePartType - Roles

            results =
                from result in root.Descendants().Elements("ownedAttribute")
                where (string)result.Attribute(ns2 + "id") != null
                where (string)result.Attribute("type") != null
                where (string)result.Attribute("name") != null
                //from result3 in root.Elements(ns + "View")
                //from result4 in root.Descendants()
                //from result2 in root.Descendants()
                //from result2 in root.Elements(ns3 + "Package").Elements("packagedElement")
                where (string)result.Attribute("aggregation") == "composite"
                //where (string)result.Attribute == (string)result2.Attribute(ns2 + "id")
                //where (string)result3.LastAttribute == (string)result4.Attribute(ns2 + "id")
                from result2 in root.Elements(ns + "ConceptRole")
                where (string)result.Attribute(ns2 + "id") == (string)result2.Attribute("base_Property")
                
                select new Thing
                {
                    type = "WholePartType",
                    id = (string)result.Attribute(ns2 + "id") + (string)result.Attribute("type"),
                    name = "$none$",
                    value = "$none$",
                    place1 = (string)result.Attribute(ns2 + "id"),
                    place2 = (string)result.Attribute("type"),
                    foundation = "WholePartType",
                    value_type = "$none$"
                };

            tuple_types = tuple_types.Concat(results.ToList());

            //OverlapType - Arbitrary

            results =
                        from result in root.Elements(ns + "ArbitraryConnector")
                        //from result3 in root.Elements(ns + "View")
                        //from result4 in root.Descendants()
                        from result2 in root.Descendants()
                        //from result2 in root.Elements(ns3 + "Package").Elements("packagedElement")
                        //where result2.Attribute("name") != null
                        where (string)result.LastAttribute == (string)result2.Attribute(ns2 + "id")
                        //where (string)result3.LastAttribute == (string)result4.Attribute(ns2 + "id")
                        from result3 in root.Descendants().Elements("ownedAttribute")
                        where (string)result3.Attribute("type") != null
                        where (string)result3.Attribute(ns2 + "id") == (string)result2.Element("supplier").Attribute(ns2 + "idref")
                        from result4 in root.Descendants().Elements("ownedAttribute")
                        where (string)result4.Attribute("type") != null
                        where (string)result4.Attribute(ns2 + "id") == (string)result2.Element("client").Attribute(ns2 + "idref")
                        
                        select new Thing
                        {
                            type = "OverlapType",
                            id = (string)result.LastAttribute,
                            name = "$none$",
                            value = (string)result2.Element("supplier").Attribute(ns2 + "idref") + (string)result2.Element("client").Attribute(ns2 + "idref"),
                            place2 = (string)result4.Attribute("type"),
                            place1 = (string)result3.Attribute("type"),
                            foundation = "CoupleType",
                            value_type = "$id$"
                        };

            tuple_types = tuple_types.Concat(results.ToList());

            //Overlap - informationAssociation

            results =
                from result in root.Descendants().Elements("ownedAttribute")
                where (string)result.Attribute("association") != null
                where (string)result.Parent.Attribute(ns2 + "id") != null
                where (string)result.Attribute("type") != null

                select new Thing
                {
                    type = "OverlapType",
                    id = (string)result.Parent.Attribute(ns2 + "id") + (string)result.Attribute("type"),
                    name = "$none$",
                    value = "$none$",
                    place1 = (string)result.Parent.Attribute(ns2 + "id"),
                    place2 = (string)result.Attribute("type"),
                    foundation = "CoupleType",
                    value_type = "$none$"
                };

            tuple_types = tuple_types.Concat(results.ToList());

            //Milestone Dates

            results =
                    from result in root.Descendants()
                    where ((string)result.Attribute("base_InstanceSpecification")) != null
                    where ((string)result.Attribute("date")) != null
                    from result2 in root.Descendants()
                    where ((string)result.Attribute("date")) == (string)result2.Attribute(ns2 + "id")
                    
                    select new Thing

                    {
                        type = "HappensInType",
                        id = (string)result.Attribute("base_InstanceSpecification") + "_t2",
                        name = "$none$",
                        value = (string)result2.Attribute("value"),
                        place1 = (string)result.Attribute("base_InstanceSpecification") + "_t1",
                        place2 = (string)result.Attribute("base_InstanceSpecification"),
                        foundation = "WholePartType",
                        value_type = "$period$"
                    };

            tuple_types = tuple_types.Concat(results.ToList());

            foreach (Thing thing in results)
            {
                values = new List<Thing>();

                values.Add(new Thing
                {
                    type = "PeriodType",
                    id = thing.place2 + "_t1",
                    name = (string)thing.value,
                    value = "$none$",
                    place1 = "$none$",
                    place2 = "$none$",
                    foundation = "IndividualType",
                    value_type = "$none$"
                });

                things = things.Concat(values);

                //values = new List<Thing>();

                //values.Add(new Thing
                //{
                //    type = "PeriodType",
                //    id = thing.place2 + "_t1",
                //    name = (string)thing.value,
                //    value = "$none$",
                //    place1 = "$none$",
                //    place2 = "$none$",
                //    foundation = "IndividualType",
                //    value_type = "$none$"
                //});

                //values.Add(thing);

                //period_dic.Add(thing.place2, values);

            }

            results =
                from result in root.Descendants()
                where ((string)result.Attribute("base_InstanceSpecification")) != null
                where ((string)result.Attribute("startBoundaryType")) != null
                where ((string)result.Attribute("endBoundaryType")) != null
                from result2 in root.Descendants()
                where ((string)result.Attribute("startBoundaryType")) == (string)result2.Attribute(ns2 + "id")
                from result3 in root.Descendants()
                where ((string)result.Attribute("endBoundaryType")) == (string)result3.Attribute(ns2 + "id")

                select new Thing

                {
                    type = "HappensInType",
                    id = (string)result.Attribute("base_InstanceSpecification") + "_t2",
                    name = "$none$",
                    value = (string)result2.Attribute("value") + " to " + (string)result3.Attribute("value"),
                    place1 = (string)result.Attribute("base_InstanceSpecification") + "_t1",
                    place2 = (string)result.Attribute("base_InstanceSpecification"),
                    foundation = "WholePartType",
                    value_type = "$period$"
                };

            tuple_types = tuple_types.Concat(results.ToList());

            foreach (Thing thing in results)
            {
                values = new List<Thing>();

                values.Add(new Thing
                {
                    type = "PeriodType",
                    id = thing.place2 + "_t1",
                    name = (string)thing.value,
                    value = "$none$",
                    place1 = "$none$",
                    place2 = "$none$",
                    foundation = "IndividualType",
                    value_type = "$none$"
                });

                things = things.Concat(values);

                //values = new List<Thing>();

                //values.Add(new Thing
                //{
                //    type = "PeriodType",
                //    id = thing.place2 + "_t1",
                //    name = (string)thing.value,
                //    value = "$none$",
                //    place1 = "$none$",
                //    place2 = "$none$",
                //    foundation = "IndividualType",
                //    value_type = "$none$"
                //});

                //values.Add(thing);

                //period_dic.Add(thing.place2, values);

            }

            ////activityPerformedByPerformer - OV-5b

            //results =
            //       from result in root.Descendants().Elements("inPartition")
            //       where (string)result.Parent.Attribute("behavior") != null
            //       from result2 in root.Descendants().Elements("group")
            //       where (string)result2.Attribute(ns2 + "id") == (string)result.Attribute(ns2 + "idref")
            //       where (string)result2.Attribute("represents") != null

            //       select new Thing

            //       {
            //           type = "activityPerformedByPerformer",
            //           id = (string)result.Parent.Attribute("represents") + (string)result.Parent.Attribute("behavior"),
            //           name = "$none$",
            //           value = "$none$",
            //           place1 = (string)result.Parent.Attribute("represents"),
            //           place2 = (string)result.Parent.Attribute("behavior"),
            //           foundation = "CoupleType",
            //           value_type = "$period$"

            //       };

            //tuple_types = tuple_types.Concat(results);


            //activityPerformedByPerformer - Milestones

            results =
                   from result in root.Elements(ns + "System")
                   where ((string)result.Attribute("milestone")) != null
                   from result2 in root.Descendants()
                   where (string)result2.Attribute(ns2 + "id") == (string)result.Attribute("base_Class")

                   select new Thing

                   {
                       type = "temp",
                       id = (string)result.Attribute("base_Class"),
                       value = ((string)result.Attribute("milestone")),
                       name = (string)result2.Attribute("name"),
                       value_type = "$milestone$"
                   };

            foreach (Thing thing in results)
            {
                foreach (string activity in ((string)thing.value).Split(' '))
                {
                    values = new List<Thing>();

                    values.Add(
                        new Thing

                        {
                            type = "activityPerformedByPerformer",
                            id = thing.id + activity,
                            name = "$none$",
                            value = "$none$",
                            place1 = thing.id,
                            place2 = activity,
                            foundation = "CoupleType",
                            value_type = "$period$"

                        }
                    );

                    tuple_types = tuple_types.Concat(values);

                }

            }

            //OV-1 Picture

            results =
                from result in root.Descendants().Elements("image")
                from result2 in root.Descendants().Elements("diagramContents").Elements("binaryObject")
                where (string)result2.Parent.Parent.Attribute("type") == "OV-1 High-Level Operational Concept Graphic"
                where (string)result.Parent.Parent.Parent.Attribute("name") == (string)result2.Attribute("streamContentID")

                select
                    //new {
                    //    key = (string)result.Parent.Parent.Attribute("SAObjId"),
                    //    value = new List<Thing> {
                        new Thing
                        {
                            type = "ArchitecturalDescription",
                            id = (string)result.Parent.Parent.Parent.Attribute("name"),
                            name = "$none$",
                            value = (string)result.Value,
                            place1 = (string)result2.Parent.Parent.Parent.Parent.Parent.Attribute(ns2 + "id"),
                            place2 = (string)result.Parent.Parent.Parent.Attribute("name"),
                            foundation = "IndividualType",
                            value_type = "exemplar"
                        };
            //}}).ToDictionary(a => a.key, a => a.value);

            OV1_pic_views = results.GroupBy(x => x.place1).ToDictionary(x => x.Key, x => x.ToList());

            if (OV1_pic_views.Count() > 0)
            {
                representation_scheme = true;
                foreach (KeyValuePair<string, List<Thing>> entry in OV1_pic_views)
                {
                    foreach (Thing thing in entry.Value)
                    {
                        tuples = tuples.Concat(new List<Thing>{new Thing
                            {
                            type = "representationSchemeInstance",
                            id = thing.id+"_1",
                            name = "$none$",
                            value = "$none$",
                            place1 = "_rs1",
                            place2 = thing.id,
                            foundation = "typeInstance",
                            value_type = "$none$"
                            }});
                    }
                }
                things = things.Concat(OV1_pic_views.SelectMany(x => x.Value));
            }

            //Diagramming

            //foreach (Thing thing in things)
            //{
            //    results_loc =
            //        from result in root.Descendants().Elements("mdElement").Elements("elementID")
            //        //from result3 in root.Elements(ns + "View")
            //        //from result4 in root.Descendants()
            //        //from result2 in root.Descendants()
            //        //from result2 in result.Elements("layoutConstraint")
            //        //where result2.Attribute("name") != null
            //        //where (string)result.Attribute(ns2 + "idref") == thing.id
            //        //where (string)result3.LastAttribute == (string)result4.Attribute(ns2 + "id")
            //        where (string)result.Attribute(ns2 + "idref") == thing.id 
            //        where ((string)result.Parent.Element("geometry").Value).Contains(";") == false
                    
            //        select new Location
            //        {
            //            id = (string)result.Parent.Attribute(ns2 + "id"),
            //            top_left_x = (string)result.Parent.Element("geometry"),//.Value).Split(',')[0],
            //            //top_left_y = ((string)result.Parent.Element("geometry").Value).Split(',')[1],
            //            //bottom_right_x = ((string)result.Parent.Element("geometry").Value).Split(',')[2],
            //            //bottom_right_y = ((string)result.Parent.Element("geometry").Value).Split(',')[3],
            //            element_id = (string)result.Attribute(ns2 + "idref")
            //        };

            //    locations = locations.Concat(results_loc.ToList());
            //}

            //foreach (Location location in locations)
            //{
            //    string[] s = location.top_left_x.Split(',');
            //    location.top_left_x = s[0];
            //    location.top_left_y = s[1];
            //    location.bottom_right_x = s[2];
            //    location.bottom_right_y = s[3];

            //}


            //foreach (Location location in locations)
            //{
            //    values = new List<Thing>();

            //    values.Add(new Thing
            //    {
            //        type = "Information",
            //        id = location.id + "_12",
            //        name = "Diagramming Information",
            //        value = "$none$",
            //        place1 = "$none$",
            //        place2 = "$none$",
            //        foundation = "IndividualType"
            //    });

            //    values.Add(new Thing
            //    {
            //        type = "Point",
            //        id = location.id + "_16",
            //        name = "Top Left Location",
            //        value = "$none$",
            //        place1 = "$none$",
            //        place2 = "$none$",
            //        foundation = "IndividualType"
            //    });

            //    values.Add(new Thing
            //    {
            //        type = "PointType",
            //        id = location.id + "_14",
            //        name = "Top Left LocationType",
            //        value = "$none$",
            //        place1 = "$none$",
            //        place2 = "$none$",
            //        foundation = "IndividualType"
            //    });

            //    values.Add(new Thing
            //    {
            //        type = "Point",
            //        id = location.id + "_26",
            //        name = "Bottome Right Location",
            //        value = "$none$",
            //        place1 = "$none$",
            //        place2 = "$none$",
            //        foundation = "IndividualType"
            //    });

            //    values.Add(new Thing
            //    {
            //        type = "PointType",
            //        id = location.id + "_24",
            //        name = "Bottome Right LocationType",
            //        value = "$none$",
            //        place1 = "$none$",
            //        place2 = "$none$",
            //        foundation = "IndividualType"
            //    });

            //    values.Add(new Thing
            //    {
            //        type = "SpatialMeasure",
            //        id = location.id + "_18",
            //        name = "Top Left X Location",
            //        value = location.top_left_x,
            //        place1 = "$none$",
            //        place2 = "$none$",
            //        foundation = "IndividualType",
            //        value_type = "numericValue"
            //    });

            //    values.Add(new Thing
            //    {
            //        type = "SpatialMeasure",
            //        id = location.id + "_20",
            //        name = "Top Left Y Location",
            //        value = location.top_left_y,
            //        place1 = "$none$",
            //        place2 = "$none$",
            //        foundation = "IndividualType",
            //        value_type = "numericValue"
            //    });

            //    values.Add(new Thing
            //    {
            //        type = "SpatialMeasure",
            //        id = location.id + "_22",
            //        name = "Top Left Z Location",
            //        value = "0",
            //        place1 = "$none$",
            //        place2 = "$none$",
            //        foundation = "IndividualType",
            //        value_type = "numericValue"
            //    });

            //    values.Add(new Thing
            //    {
            //        type = "SpatialMeasure",
            //        id = location.id + "_28",
            //        name = "Bottom Right X Location",
            //        value = location.bottom_right_x,
            //        place1 = "$none$",
            //        place2 = "$none$",
            //        foundation = "IndividualType",
            //        value_type = "numericValue"
            //    });

            //    values.Add(new Thing
            //    {
            //        type = "SpatialMeasure",
            //        id = location.id + "_30",
            //        name = "Bottom Right Y Location",
            //        value = location.bottom_right_y,
            //        place1 = "$none$",
            //        place2 = "$none$",
            //        foundation = "IndividualType",
            //        value_type = "numericValue"
            //    });

            //    values.Add(new Thing
            //    {
            //        type = "SpatialMeasure",
            //        id = location.id + "_32",
            //        name = "Bottom Right Z Location",
            //        value = "0",
            //        place1 = "$none$",
            //        place2 = "$none$",
            //        foundation = "IndividualType",
            //        value_type = "numericValue"
            //    });

            //    things = things.Concat(values);

            //    values = new List<Thing>();

            //    values.Add(new Thing
            //    {
            //        type = "describedBy",
            //        id = location.id + "_11",
            //        name = "$none$",
            //        value = "$none$",
            //        place1 = location.element_id,
            //        place2 = location.id + "_12",
            //        foundation = "namedBy"
            //    });

            //    values.Add(new Thing
            //    {
            //        type = "typeInstance",
            //        id = location.id + "_15",
            //        name = "$none$",
            //        value = "$none$",
            //        place1 = location.id + "_14",
            //        place2 = location.id + "_16",
            //        foundation = "typeInstance"
            //    });

            //    values.Add(new Thing
            //    {
            //        type = "typeInstance",
            //        id = location.id + "_25",
            //        name = "$none$",
            //        value = "$none$",
            //        place1 = location.id + "_24",
            //        place2 = location.id + "_26",
            //        foundation = "typeInstance"
            //    });


            //    values.Add(new Thing
            //    {
            //        type = "measureOfIndividualPoint",
            //        id = location.id + "_17",
            //        name = "$none$",
            //        value = "$none$",
            //        place1 = location.id + "_18",
            //        place2 = location.id + "_16",
            //        foundation = "typeInstance"
            //    });

            //    values.Add(new Thing
            //    {
            //        type = "measureOfIndividualPoint",
            //        id = location.id + "_19",
            //        name = "$none$",
            //        value = "$none$",
            //        place1 = location.id + "_20",
            //        place2 = location.id + "_16",
            //        foundation = "typeInstance"
            //    });

            //    values.Add(new Thing
            //    {
            //        type = "measureOfIndividualPoint",
            //        id = location.id + "_21",
            //        name = "$none$",
            //        value = "$none$",
            //        place1 = location.id + "_22",
            //        place2 = location.id + "_16",
            //        foundation = "typeInstance"
            //    });

            //    values.Add(new Thing
            //    {
            //        type = "measureOfIndividualPoint",
            //        id = location.id + "_27",
            //        name = "$none$",
            //        value = "$none$",
            //        place1 = location.id + "_28",
            //        place2 = location.id + "_26",
            //        foundation = "typeInstance"
            //    });

            //    values.Add(new Thing
            //    {
            //        type = "measureOfIndividualPoint",
            //        id = location.id + "_29",
            //        name = "$none$",
            //        value = "$none$",
            //        place1 = location.id + "_30",
            //        place2 = location.id + "_26",
            //        foundation = "typeInstance"
            //    });

            //    values.Add(new Thing
            //    {
            //        type = "measureOfIndividualPoint",
            //        id = location.id + "_31",
            //        name = "$none$",
            //        value = "$none$",
            //        place1 = location.id + "_32",
            //        place2 = location.id + "_26",
            //        foundation = "typeInstance"
            //    });

            //    tuples = tuples.Concat(values);

            //    values = new List<Thing>();

            //    values.Add(new Thing
            //    {
            //        type = "resourceInLocationType",
            //        id = location.id + "_13",
            //        name = "$none$",
            //        value = "$none$",
            //        place1 = location.id + "_12",
            //        place2 = location.id + "_14",
            //        foundation = "CoupleType"
            //    });

            //    values.Add(new Thing
            //    {
            //        type = "resourceInLocationType",
            //        id = location.id + "_23",
            //        name = "$none$",
            //        value = "$none$",
            //        place1 = location.id + "_12",
            //        place2 = location.id + "_24",
            //        foundation = "CoupleType"
            //    });

            //    tuple_types = tuple_types.Concat(values);
            //}

            //Thing Dictionary

            var duplicateKeys = things.GroupBy(x => x.id)
                            .Where(group => group.Count() > 1)
                            .Select(group => group.Key);

            things_dic = things.ToDictionary(x => x.id, x => x);

            //ACR, APR

            results =
                    from result in root.Descendants().Elements("conveyed")
                    //
                    from result2 in root.Descendants().Elements("informationSource")
                    where (string)result.Parent.Attribute(ns2 + "id") == (string)result2.Parent.Attribute(ns2 + "id")
                    from result3 in root.Descendants().Elements("informationTarget")
                    where (string)result3.Parent.Attribute(ns2 + "id") == (string)result.Parent.Attribute(ns2 + "id")
                    //
                    from result4 in root.Descendants().Elements("realizingActivityEdge")

                    where (string)result.Parent.Attribute(ns2 + "id") == (string)result4.Parent.Attribute(ns2 + "id")
                    from result5 in root.Descendants().Elements("outgoing")
                    where (string)result5.Parent.Attribute("behavior") != null
                    where (string)result4.Attribute(ns2 + "idref") == (string)result5.Attribute(ns2 + "idref")

                    from result6 in root.Descendants().Elements("incoming")
                    where (string)result6.Parent.Attribute("behavior") != null
                    where (string)result4.Attribute(ns2 + "idref") == (string)result6.Attribute(ns2 + "idref")


                    select new Thing
                    {
                        type = (string)result.Attribute(ns2 + "idref"),
                        id = (string)result2.Attribute(ns2 + "idref"),
                        name = "$none$",
                        value = (string)result3.Attribute(ns2 + "idref"),
                        place1 = (string)result5.Parent.Attribute("behavior"),
                        place2 = (string)result6.Parent.Attribute("behavior"),
                        value_type = "$performer$"
                    };

            foreach (Thing thing in results)
            {
                tuple_types = tuple_types.Concat(new List<Thing>(){
                        new Thing
                        {
                            type = "activityProducesResource",
                            id = thing.place1 + thing.type + "_d1" + "_1",
                            name = thing.name,
                            value = "$none$",
                            place1 = thing.place1,
                            place2 = thing.type + "_d1",
                            foundation = "CoupleType",
                            value_type = "$none$"
                        }
                    });

                tuple_types = tuple_types.Concat(new List<Thing>(){
                        new Thing
                        {
                            type = "activityConsumesResource",
                            id = thing.type + "_d1" + thing.place2 + "_2",
                            name = thing.name,
                            value = "$none$",
                            place1 = thing.type + "_d1",
                            place2 = thing.place2,
                            foundation = "CoupleType",
                            value_type = "$none$"
                        }
                    });

                tuple_types = tuple_types.Concat(new List<Thing>(){
                        new Thing
                        {
                            type = "activityProducesResource",
                            id = thing.place1 + thing.type + "_3",
                            name = thing.name,
                            value = "$none$",
                            place1 = thing.place1,
                            place2 = thing.type,
                            foundation = "CoupleType",
                            value_type = "$none$"
                        }
                    });

                tuple_types = tuple_types.Concat(new List<Thing>(){
                        new Thing
                        {
                            type = "activityConsumesResource",
                            id = thing.type + thing.place2 + "_4",
                            name = thing.name,
                            value = "$none$",
                            place1 = thing.type,
                            place2 = thing.place2,
                            foundation = "CoupleType",
                            value_type = "$none$"
                        }
                    });

                if (!things_dic.TryGetValue(thing.type + "_d1", out value2))
                    if (things_dic.TryGetValue(thing.type, out value))
                    {

                        things = things.Concat(new List<Thing>(){
                        new Thing
                            {
                                type = "Data",
                                id = value.id + "_d1",
                                name = value.name,
                                value = "$none$",
                                place1 = "$none$",
                                place2 = "$none$",
                                foundation = "IndividualType",
                                value_type = "$none$"
                            }
                        });

                        things_dic.Add(value.id + "_d1", new Thing
                        {
                            type = "Data",
                            id = value.id + "_d1",
                            name = value.name,
                            value = "$none$",
                            place1 = "$none$",
                            place2 = "$none$",
                            foundation = "IndividualType",
                            value_type = "$none$"
                        });
                    }

            }

            tuple_types = tuple_types.GroupBy(x => x.id).Select(grp => grp.First());
            things_dic = things.ToDictionary(x => x.id, x => x);

            ////Need line

            //results_dic2 = new Dictionary<string, List<Thing>>();
            //results_dic3 = new Dictionary<string, List<Thing>>();
            //results_dic = tuple_types.Where(x => x.type == "activityPerformedByPerformer").GroupBy(x => x.place1).ToDictionary(x => x.Key, x => x.ToList());
            //aro = tuple_types.Where(x => x.type == "activityConsumesResource").GroupBy(x => x.place2).ToDictionary(x => x.Key, x => x.ToList());

            //results =
            //                        from result in root.Elements(ns + "Performer")
            //                        from result2 in root.Descendants().Elements("supplier")
            //                        where (string)result.Attribute("base_Class") == (string)result2.Attribute(ns2 + "idref")
            //                        from result3 in root.Descendants().Elements("client")
            //                        where (string)result3.Parent.Attribute(ns2 + "id") == (string)result2.Parent.Attribute(ns2 + "id")
            //                        from result4 in root.Elements(ns + "Performer")
            //                        where (string)result4.Attribute("base_Class") == (string)result3.Attribute(ns2 + "idref")

            //        select new Thing
            //        {
            //            type = "Resource Flow",
            //            id = (string)result.Parent.Parent.Attribute("SAObjId"),
            //            name = "none",
            //            value = (string)result.Parent.Attribute("SAPrpName") + "_" + (string)result.Parent.Parent.Attribute("SAObjMinorTypeName"),
            //            place1 = (string)result.Parent.Parent.Attribute("SAObjId"),
            //            place2 = (string)result.Attribute("SALinkIdentity"),
            //            foundation = "CoupleType",
            //            value_type = "$none$"
            //        };

            //foreach (Thing thing in results.ToList())
            //{
            //    if (results_dic.TryGetValue(thing.place2, out values))
            //    {
            //        values2 = new List<Thing>();
            //        values4 = new List<Thing>();
            //        add = true;
            //        foreach (Thing app in values)
            //        {
            //            if (aro.TryGetValue(app.place2, out values3))
            //            {
            //                add = false;
            //                //break;
            //                values4.Add(app);
            //                values2.AddRange(values3);
            //            }
            //        }
            //        if (add)
            //            errors_list.Add("Definition error," + thing.id + "," + thing.name + "," + thing.type + ",Missing Mandatory Element: activityConsumesResource\r\n");
            //    }
            //    else
            //    {
            //        errors_list.Add("Definition error," + thing.id + "," + thing.name + "," + thing.type + ",Missing Mandatory Element: activityPerformedByPerformer\r\n");
            //    }

            //    results_dic3.Add(thing.id, values4);
            //    results_dic2.Add(thing.id, values2);

            //}

            //if (results_dic2.Count > 0)
            //{
            //    aro2 = tuple_types.Where(x => x.type == "activityProducesResource").GroupBy(x => x.place1).ToDictionary(x => x.Key, x => x.ToList());

            //    results =
            //            from result in root.Elements("Class").Elements("SADefinition").Elements("SAProperty").Elements("SALink")
            //            where (string)result.Parent.Parent.Attribute("SAObjMinorTypeName") != "System Exchange (DM2rx)" && (string)result.Parent.Parent.Attribute("SAObjMinorTypeName") != "Operational Exchange (DM2rx)"
            //                && (string)result.Parent.Parent.Attribute("SAObjMinorTypeName") != "System Data Flow (DM2rx)" && (string)result.Parent.Parent.Attribute("SAObjMinorTypeName") != "Service Data Flow (DM2rx)"
            //            where (string)result.Parent.Attribute("SAPrpName") == "performerSource" || (string)result.Parent.Attribute("SAPrpName") == "Source"
            //            from result2 in root.Elements("Class").Elements("SADefinition")
            //            where (string)result.Attribute("SALinkIdentity") == (string)result2.Attribute("SAObjId")
            //            select new Thing
            //            {
            //                type = "Resource Flow",
            //                id = (string)result.Parent.Parent.Attribute("SAObjId"),
            //                name = ((string)result.Parent.Parent.Attribute("SAObjName")).Replace("&", " And "),
            //                value = (string)result.Parent.Attribute("SAPrpName") + "_" + (string)result.Parent.Parent.Attribute("SAObjMinorTypeName"),
            //                place1 = (string)result.Parent.Parent.Attribute("SAObjId"),
            //                place2 = (string)result.Attribute("SALinkIdentity"),
            //                foundation = "CoupleType",
            //                value_type = "$none$"
            //            };

            //    foreach (Thing thing in results.ToList())
            //    {
            //        results_dic2.TryGetValue(thing.id, out values4);
            //        if (values4 == null)
            //            continue;
            //        values_dic = values4.ToDictionary(x => "_" + x.id.Split('_')[1], x => x);
            //        if (results_dic.TryGetValue(thing.place2, out values))
            //        {
            //            add = true;
            //            values2 = new List<Thing>();
            //            values7 = new List<Thing>();
            //            foreach (Thing app in values)
            //            {
            //                if (aro2.TryGetValue(app.place2, out values3))
            //                {
            //                    foreach (Thing apr in values3)
            //                    {
            //                        if (values_dic.TryGetValue("_" + apr.id.Split('_')[1], out value))
            //                        {
            //                            add = false;
            //                            //break;
            //                            values2.Add(value);
            //                            values7.Add(things_dic[value.place1]);
            //                            values2.Add(things_dic[value.place2]);
            //                            values2.Add(app);
            //                            value2 = things_dic[app.place1];
            //                            if (value2.type == "Performer")
            //                                values7.Add(value2);
            //                            else
            //                                values2.Add(value2);
            //                            values2.Add(apr);
            //                            values2.Add(things_dic[apr.place1]);
            //                            results_dic3.TryGetValue(thing.id, out values5);
            //                            values6 = values5.Where(x => x.place2 == value.place2).ToList();
            //                            values2.AddRange(values6);
            //                            foreach (Thing app2 in values6)
            //                            {
            //                                value2 = things_dic[app2.place1];
            //                                if (value2.type == "Performer")
            //                                    values7.Add(value2);
            //                                else
            //                                    values2.Add(value2);

            //                                tuple_types = tuple_types.Concat(new List<Thing>()
            //                                {
            //                                    new Thing
            //                                    {
            //                                        type = "OverlapType",
            //                                        id = thing.id,
            //                                        name = thing.name,
            //                                        value = "$none$",
            //                                        place1 = app.place1,
            //                                        place2 = app2.place1,
            //                                        foundation = "CoupleType",
            //                                        value_type = "$none$"
            //                                    }
            //                                });
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //            if (add)
            //                errors_list.Add("Definition error," + thing.id + "," + thing.name + "," + thing.type + ",Missing Mandatory Element: activityProducesResource\r\n");
            //        }
            //        else
            //        {
            //            errors_list.Add("Definition error," + thing.id + "," + thing.name + "," + thing.type + ",Missing Mandatory Element: activityPerformedByPerformer\r\n");
            //        }

            //        if (values2.Count > 0)
            //            needline_mandatory_views.Add(thing.id, values2);

            //        if (values7.Count > 0)
            //            needline_optional_views.Add(thing.id, values7);

            //    }
            //}

            //SV-7 / SvcV-7

            results =
                from result in root.Descendants().Elements("ownedAttribute")
                where (string)result.Attribute("aggregation") == "composite"
                where (string)result.Parent.Attribute(ns2 + "id") != null
                where (string)result.Attribute(ns2 + "id") != null
                from result3 in root.Elements(ns + "Measurement")
                where (string)result3.Attribute("base_Property") == (string)result.Attribute(ns2 + "id")
                //from result2 in root.Descendants()
                //where (string)result2.Attribute("propertySet") != null
                //where (string)result2.Attribute("propertySet") == (string)result.Parent.Attribute(ns2 + "id")
                //where (string)result2.Attribute("base_Class") != null


                select new Thing
                {
                    type = "typeInstance",
                    id = (string)result.Parent.Attribute(ns2 + "id") + (string)result.Attribute(ns2 + "id"),
                    name = "$none$",
                    value = "$none$",
                    place1 = (string)result.Parent.Attribute(ns2 + "id"),
                    place2 = (string)result.Attribute(ns2 + "id"),
                    foundation = "typeInstance",
                    value_type = "$none$"
                };

            tuples = tuples.Concat(results.ToList());

            foreach (Thing thing in results)
            {

                if (lookup.TryGetValue(thing.place1, out values))
                {
                    values2 = new List<Thing>();
                    values2.AddRange(values);
                    values2.Add(things_dic[thing.place2]);
                    lookup.Remove(thing.place1);
                    lookup.Add(thing.place1, values2);
                }
                else
                {
                    values = new List<Thing>();
                    values.Add(things_dic[thing.place1]);
                    values.Add(things_dic[thing.place2]);
                    lookup.Add(thing.place1, values);
                }
            }

            ////

            results =
                from result in root.Descendants().Elements("ownedAttribute")
                where (string)result.Attribute("aggregation") == "composite"
                where (string)result.Parent.Attribute(ns2 + "id") != null
                where (string)result.Attribute(ns2 + "id") != null
                from result3 in root.Elements(ns + "Measurement")
                where (string)result3.Attribute("base_Property") == (string)result.Attribute(ns2 + "id")
                from result2 in root.Descendants()
                where (string)result2.Attribute("propertySet") != null
                where ((string)result2.Attribute("propertySet")).Contains((string)result.Parent.Attribute(ns2 + "id"))
                where (string)result2.Attribute("base_Class") != null


                select new Thing
                {
                    type = "measureOfTypeResource",
                    id = (string)result.Attribute(ns2 + "id") + (string)result2.Attribute("base_Class"),
                    name = "$none$",
                    value = (string)result.Parent.Attribute(ns2 + "id"),
                    place1 = (string)result.Attribute(ns2 + "id"),
                    place2 = (string)result2.Attribute("base_Class"),
                    foundation = "superSubtype",
                    value_type = "$id$"
                };

            tuples = tuples.Concat(results.ToList());

            foreach (Thing thing in results)
            {
                if (lookup.TryGetValue((string)thing.value, out values))
                {
                    values2 = new List<Thing>();
                    values2.AddRange(values);
                    values2.Add(things_dic[thing.place2]);
                    lookup.Remove((string)thing.value);
                    lookup.Add((string)thing.value, values2);
                }
            }

            //CV-6

            mandatory_list = new List<Thing>();
            values = new List<Thing>();
            optional_list = new List<Thing>();
            sorted_results = new List<List<Thing>>();

            //values_dic = things_dic.Where(x => x.Value.type == "Activity").ToDictionary(p => p.Key, p => p.Value);
            //values_dic2 = things_dic.Where(x => x.Value.type == "Capability").ToDictionary(p => p.Key, p => p.Value);

            //results = tuple_types.Where(x => x.type == "activityPartOfCapability").Where(x => values_dic2.ContainsKey(x.place1)).Where(x => values_dic.ContainsKey(x.place2));

            foreach (Thing thing in things.Where(x => x.type == "Activity").ToList())
            {
                //mandatory_list.Add(new Thing { id = thing.id, type = thing.type, value = "$none$", value_type = "$none$" });
                values.Add(new Thing { id = "_13", type = "CV-6", place2 = thing.id, value = thing.type, place1 = "_13" });
            }

            foreach (Thing thing in things.Where(x => x.type == "Capability").ToList())
            {
                //mandatory_list.Add(new Thing { id = thing.id, type = thing.type, value = "$none$", value_type = "$none$" });
                values.Add(new Thing { id = "_13", type = "CV-6", place2 = thing.id, value = thing.type, place1 = "_13" });
            }

            sorted_results.Add(values);

            sorted_results_new = new List<List<Thing>>();
            Add_Tuples(ref sorted_results, ref sorted_results_new, tuples.ToList(), ref errors_list);
            Add_Tuples(ref sorted_results, ref sorted_results_new, tuple_types.ToList(), ref errors_list);
            sorted_results = sorted_results_new;

            foreach (Thing thing in sorted_results.First())
            {
                if ((string)thing.value == "Activity" || (string)thing.value == "Capability" || (string)thing.value == "activityPartOfCapability")
                    mandatory_list.Add(new Thing { id = thing.place2, type = (string)thing.value, value = "$none$", value_type = "$none$" });
                else
                    optional_list.Add(new Thing { id = thing.place2, type = (string)thing.value, value = "$none$", value_type = "$none$" });
            }

            if (sorted_results.First().Count() > 0)
                views.Add(new View { type = "CV-6", id = "_13", name = "NEAR CV-6", optional = optional_list, mandatory = mandatory_list });

            //PV-2

            mandatory_list = new List<Thing>();
            values = new List<Thing>();
            values2 = new List<Thing>();
            optional_list = new List<Thing>();
            sorted_results = new List<List<Thing>>();

                results =
                    //from result in root.Descendants().Elements("value")
                    from result2 in root.Elements(ns+"Project")
                    //where (string)result.Parent.Parent.Attribute(ns2 + "id") == (string)result2.Attribute("base_InstanceSpecification")
                    //from result3 in root.Descendants()
                    //where (string)result.Attribute("instance") == (string)result3.Attribute("base_InstanceSpecification")
                    //from result4 in root.Descendants()
                    //where (string)result4.Attribute(ns2+"id") == (string)result3.Attribute("date")
                    from result5 in root.Descendants().Elements("classifier")
                    from result7 in root.Elements(ns + "ProjectType")
                    where (string)result5.Attribute(ns2 + "idref") == (string)result7.Attribute("base_Class")
                    where (string)result5.Parent.Attribute(ns2 + "id") == (string)result2.Attribute("base_InstanceSpecification")


                    select new Thing
                    {
                        type = (string)result5.Attribute(ns2 + "idref"),
                        //id = (string)result3.Attribute("date"),
                        name = "$none$",
                        //value = (string)result4.Attribute("value"),
                        place1 = (string)result2.Attribute("base_InstanceSpecification"),
                        //place2 = (string)result.Attribute("instance"),
                        value_type = "$date$"
                    };

                foreach (Thing thing in results)
                {
                    //things = things.Concat(new List<Thing>(){
                    //    new Thing
                    //    {
                    //        type = "PeriodType",
                    //        id = thing.place2 + thing.type + "_t1",
                    //        name = (string)thing.value,
                    //        value = "$none$",
                    //        place1 = "$none$",
                    //        place2 = "$none$",
                    //        foundation = "IndividualType",
                    //        value_type = "$none$"
                    //    }
                    //});

                    //tuple_types = tuple_types.Concat(new List<Thing>(){
                    //    new Thing
                    //    {
                    //        type = "HappensInType",
                    //        id = thing.place2 + thing.type + "_t2",
                    //        name = "$none$",
                    //        value = (string)thing.value,
                    //        place1 = thing.place2 + thing.type + "_t1",
                    //        place2 = thing.place2,
                    //        foundation = "WholePartType",
                    //        value_type = "$period$"
                    //    }
                    //});

                    tuple_types = tuple_types.Concat(new List<Thing>(){
                        new Thing
                        {
                            type = "typeInstance",
                            id = thing.place1 + thing.type + "_ti1",
                            name = "$none$",
                            value = "$none$",
                            place1 = thing.type,
                            place2 = thing.place1,
                            foundation = "typeInstance",
                            value_type = "$none$"
                        }
                    });

                    //tuple_types = tuple_types.Concat(new List<Thing>(){
                    //    new Thing
                    //    {
                    //        type = "activityPartOfProjectType",
                    //        id = thing.place2 + thing.type + "_apt1",
                    //        name = "$none$",
                    //        value = "$none$",
                    //        place1 = thing.type,
                    //        place2 = thing.place2,
                    //        foundation = "typeInstance",
                    //        value_type = "$none$"
                    //    }
                   // });

                    values.Add(new Thing { id = "_14", type = "PV-2", place2 = thing.place1, value = "Project", place1 = "_14" });
                    values.Add(new Thing { id = "_14", type = "PV-2", place2 = thing.type, value = "ProjectType", place1 = "_14" });
                    //values.Add(new Thing { id = "_14", type = "PV-2", place2 = thing.place2, value = "Activity", place1 = "_14" });
                    //values.Add(new Thing { id = "_14", type = "PV-2", place2 = thing.place2 + thing.type + "_t2", value = "HappensInType", place1 = "_14" });
                    //values.Add(new Thing { id = "_14", type = "PV-2", place2 = thing.place2 + thing.type + "_t1", value = "PeriodType", place1 = "_14" });
                    //values.Add(new Thing { id = "_14", type = "PV-2", place2 = thing.place1 + thing.type + "_ti1", value = "typeInstance", place1 = "_14" });
                    //values.Add(new Thing { id = "_14", type = "PV-2", place2 = thing.place2 + thing.type + "_apt1", value = "activityPartOfProjectType", place1 = "_14" });

                }

            //

                results =
                    from result in root.Descendants().Elements("value")
                    from result2 in root.Elements(ns + "Project")
                    where (string)result.Parent.Parent.Attribute(ns2 + "id") == (string)result2.Attribute("base_InstanceSpecification")
                    from result3 in root.Descendants()
                    where (string)result.Attribute("instance") == (string)result3.Attribute("base_InstanceSpecification")
                    from result4 in root.Descendants()
                    where (string)result4.Attribute(ns2 + "id") == (string)result3.Attribute("date")
                    from result5 in root.Descendants().Elements("classifier")
                    from result7 in root.Elements(ns + "ProjectType")
                    where (string)result5.Attribute(ns2 + "idref") == (string)result7.Attribute("base_Class")
                    where (string)result5.Parent.Attribute(ns2 + "id") == (string)result2.Attribute("base_InstanceSpecification")

                    select new Thing
                    {
                        type = (string)result5.Attribute(ns2 + "idref"),
                        id = (string)result3.Attribute("date"),
                        name = "$none$",
                        value = (string)result4.Attribute("value"),
                        place1 = (string)result2.Attribute("base_InstanceSpecification"),
                        place2 = (string)result.Attribute("instance"),
                        value_type = "$date$"
                    };

                foreach (Thing thing in results)
                {
                    //things = things.Concat(new List<Thing>(){
                    //    new Thing
                    //    {
                    //        type = "PeriodType",
                    //        id = thing.place2 + thing.type + "_t1",
                    //        name = (string)thing.value,
                    //        value = "$none$",
                    //        place1 = "$none$",
                    //        place2 = "$none$",
                    //        foundation = "IndividualType",
                    //        value_type = "$none$"
                    //    }
                    //});

                    //tuple_types = tuple_types.Concat(new List<Thing>(){
                    //    new Thing
                    //    {
                    //        type = "HappensInType",
                    //        id = thing.place2 + thing.type + "_t2",
                    //        name = "$none$",
                    //        value = (string)thing.value,
                    //        place1 = thing.place2 + thing.type + "_t1",
                    //        place2 = thing.place2,
                    //        foundation = "WholePartType",
                    //        value_type = "$period$"
                    //    }
                    //});

                    //tuple_types = tuple_types.Concat(new List<Thing>(){
                    //    new Thing
                    //    {
                    //        type = "typeInstance",
                    //        id = thing.place1 + thing.type + "_ti1",
                    //        name = "$none$",
                    //        value = "$none$",
                    //        place1 = thing.type,
                    //        place2 = thing.place1,
                    //        foundation = "typeInstance",
                    //        value_type = "$none$"
                    //    }
                    //});

                    tuple_types = tuple_types.Concat(new List<Thing>(){
                        new Thing
                        {
                            type = "activityPartOfProjectType",
                            id = thing.place2 + thing.type + "_apt1",
                            name = "$none$",
                            value = "$none$",
                            place1 = thing.type,
                            place2 = thing.place2,
                            foundation = "WholePartType",
                            value_type = "$none$"
                        }
                    });

                    //values.Add(new Thing { id = "_14", type = "PV-2", place2 = thing.place1, value = "Project", place1 = "_14" });
                    //values.Add(new Thing { id = "_14", type = "PV-2", place2 = thing.type, value = "ProjectType", place1 = "_14" });
                    values.Add(new Thing { id = "_14", type = "PV-2", place2 = thing.place2, value = "Activity", place1 = "_14" });
                    //values.Add(new Thing { id = "_14", type = "PV-2", place2 = thing.place2 + thing.type + "_t2", value = "HappensInType", place1 = "_14" });
                    values.Add(new Thing { id = "_14", type = "PV-2", place2 = thing.place2 + "_t1", value = "PeriodType", place1 = "_14" });
                    //values.Add(new Thing { id = "_14", type = "PV-2", place2 = thing.place1 + thing.type + "_ti1", value = "typeInstance", place1 = "_14" });
                    //values.Add(new Thing { id = "_14", type = "PV-2", place2 = thing.place2 + thing.type + "_apt1", value = "activityPartOfProjectType", place1 = "_14" });

                }

            //
                sorted_results.Add(values);

                sorted_results_new = new List<List<Thing>>();
                Add_Tuples(ref sorted_results, ref sorted_results_new, tuples.ToList(), ref errors_list);
                Add_Tuples(ref sorted_results, ref sorted_results_new, tuple_types.ToList(), ref errors_list);
                sorted_results = sorted_results_new;

                foreach (Thing thing in sorted_results.First())
                {
                    if ((string)thing.value == "Activity" || (string)thing.value == "Project" || (string)thing.value == "activityPartOfProjectType" || (string)thing.value == "ProjectType")
                        mandatory_list.Add(new Thing { id = thing.place2, type = (string)thing.value, value = "$none$", value_type = "$none$" });
                    else
                        optional_list.Add(new Thing { id = thing.place2, type = (string)thing.value, value = "$none$", value_type = "$none$" });
                }

                if (sorted_results.First().Count() > 0)
                    views.Add(new View { type = "PV-2", id = "_14", name = "NEAR PV-2", optional = optional_list, mandatory = mandatory_list });

            ////OV-2

            //    mandatory_list = new List<Thing>();
            //    values = new List<Thing>();
            //    optional_list = new List<Thing>();
            //    sorted_results = new List<List<Thing>>();

            //    values_dic2 = things_dic.Where(x => x.Value.type == "Resource").ToDictionary(p => p.Key, p => p.Value);

            //    results = tuple_types.Where(x => x.type == "activityConsumesResource").Where(x => values_dic2.ContainsKey(x.place1));
            //    values_dic = tuple_types.Where(x => x.type == "activityProducesResource").GroupBy(x => x.place2).Select(grp => grp.First()).ToDictionary(x => x.place2, x => x);

            //    foreach (Thing rela in results)
            //    {
            //        if (values_dic.TryGetValue(rela.place1, out value))
            //        {
            //            values.Add(rela);
            //            values.Add(value);
            //        }

            //    }

            //    count = 0;
            //    count2 = values.Count();

            //    //var duplicateKeys = app2.GroupBy(x => x.place2)
            //    //            .Where(group => group.Count() > 1)
            //    //            .Select(group => group.Key);

            //    //List<string> test = duplicateKeys.ToList();

            //    values_dic2 = tuple_types.Where(x => x.type == "activityPerformedByPerformer").Where(x => Allowed_Element("OV-2", x.place1, ref things_dic)).GroupBy(x => x.place2).Select(grp => grp.First()).ToDictionary(x => x.place2, x => x);

            //    while (count < count2)
            //    {
            //        add = false;

            //        foreach (Thing thing in values)
            //        {
            //            if (values_dic2.TryGetValue(values[count].place2, out value))
            //                if (values_dic2.TryGetValue(values[count + 1].place1, out value2))
            //                {
            //                    add = true;
            //                    values.Add(value);
            //                    values.Add(value2);
            //                    break;
            //                }
            //        }


            //        if (add == true)
            //        {
            //            count = count + 2;
            //        }
            //        else
            //        {
            //            values.RemoveAt(count);
            //            values.RemoveAt(count);
            //            count2 = count2 - 2;
            //        }
            //    }

            //    sorted_results.Add(Add_Places(things_dic, values));

            //    foreach (Thing thing in sorted_results.First())
            //    {
            //        temp = Find_Mandatory_Optional(thing.type, "OV-2", "OV-2", "_21", ref errors_list);
            //        if (temp == "Mandatory")
            //            mandatory_list.Add(new Thing { id = thing.id, type = thing.type, value = "$none$", value_type = "$none$" });
            //        if (temp == "Optional")
            //            optional_list.Add(new Thing { id = thing.id, type = thing.type, value = "$none$", value_type = "$none$" });
            //    }

            //    if (sorted_results.First().Count() > 0)
            //        views.Add(new View { type = "OV-2", id = "_21", name = "NEAR OV-2", optional = optional_list, mandatory = mandatory_list });

            ////SV-6

            //    mandatory_list = new List<Thing>();
            //    values = new List<Thing>();
            //    optional_list = new List<Thing>();
            //    sorted_results = new List<List<Thing>>();

            //    values_dic2 = things_dic.Where(x => x.Value.type == "Data").ToDictionary(p => p.Key, p => p.Value);

            //    results = tuple_types.Where(x => x.type == "activityConsumesResource").Where(x => values_dic2.ContainsKey(x.place1));
            //    values_dic = tuple_types.Where(x => x.type == "activityProducesResource").GroupBy(x => x.place2).Select(grp => grp.First()).ToDictionary(x => x.place2, x => x);

            //    foreach (Thing rela in results)
            //    {
            //        if (values_dic.TryGetValue(rela.place1, out value))
            //        {
            //            values.Add(rela);
            //            values.Add(value);
            //        }

            //    }

            //    count = 0;
            //    count2 = values.Count();

            //    //var duplicateKeys = app2.GroupBy(x => x.place2)
            //    //            .Where(group => group.Count() > 1)
            //    //            .Select(group => group.Key);

            //    //List<string> test = duplicateKeys.ToList();

            //    values_dic2 = tuple_types.Where(x => x.type == "activityPerformedByPerformer").Where(x => Allowed_Element("SV-6", x.place1, ref things_dic)).GroupBy(x => x.place2).Select(grp => grp.First()).ToDictionary(x => x.place2, x => x);

            //    while (count < count2)
            //    {
            //        add = false;

            //        foreach (Thing thing in values)
            //        {
            //            if (values_dic2.TryGetValue(values[count].place2, out value))
            //                if (values_dic2.TryGetValue(values[count + 1].place1, out value2))
            //                {
            //                    add = true;
            //                    values.Add(value);
            //                    values.Add(value2);
            //                    break;
            //                }
            //        }


            //        if (add == true)
            //        {
            //            count = count + 2;
            //        }
            //        else
            //        {
            //            values.RemoveAt(count);
            //            values.RemoveAt(count);
            //            count2 = count2 - 2;
            //        }
            //    }

            //    sorted_results.Add(Add_Places(things_dic, values));

            //    foreach (Thing thing in sorted_results.First())
            //    {
            //        temp = Find_Mandatory_Optional(thing.type, "SV-6", "SV-6", "_15", ref errors_list);
            //        if (temp == "Mandatory")
            //            mandatory_list.Add(new Thing { id = thing.id, type = thing.type, value = "$none$", value_type = "$none$" });
            //        if (temp == "Optional")
            //            optional_list.Add(new Thing { id = thing.id, type = thing.type, value = "$none$", value_type = "$none$" });
            //    }

            //    if (sorted_results.First().Count() > 0)
            //        views.Add(new View { type = "SV-6", id = "_15", name = "NEAR SV-6", optional = optional_list, mandatory = mandatory_list });

            //CV-3

                mandatory_list = new List<Thing>();
                values = new List<Thing>();
                optional_list = new List<Thing>();
                sorted_results = new List<List<Thing>>();

                results =
                        from result in root.Elements(ns + "Capability")
                        from result2 in root.Descendants().Elements("supplier")
                        where (string)result.Attribute("base_Class") == (string)result2.Attribute(ns2 + "idref")
                        from result3 in root.Descendants().Elements("client")
                        where (string)result3.Parent.Attribute(ns2 + "id") == (string)result2.Parent.Attribute(ns2 + "id")
                        from result4 in root.Elements(ns + "OperationalActivity")
                        where (string)result4.Attribute("base_Activity") == (string)result3.Attribute(ns2 + "idref")
                        from result5 in root.Elements(ns + "Project")
                        from result6 in root.Descendants().Elements("supplier")
                        where (string)result5.Attribute("base_InstanceSpecification") == (string)result6.Attribute(ns2 + "idref")
                        from result7 in root.Descendants().Elements("client")
                        where (string)result6.Parent.Attribute(ns2 + "id") == (string)result7.Parent.Attribute(ns2 + "id")
                        where (string)result4.Attribute("base_Activity") == (string)result7.Attribute(ns2 + "idref")

                        from result8 in root.Descendants().Elements("classifier")
                        from result9 in root.Elements(ns + "ProjectType")
                        where (string)result8.Attribute(ns2 + "idref") == (string)result9.Attribute("base_Class")
                        where (string)result8.Parent.Attribute(ns2 + "id") == (string)result5.Attribute("base_InstanceSpecification")

                        //from result11 in root.Descendants()
                        //where (string)result5.Attribute("startDate") == (string)result11.Attribute(ns2 + "id")
                        //from result12 in root.Descendants()
                        //where (string)result5.Attribute("startDate") == (string)result12.Attribute(ns2 + "id")


                        select new Thing
                        {
                            type = (string)result9.Attribute("base_Class"),
                            id = (string)result5.Attribute("base_InstanceSpecification"),
                            name = "$none$",
                            //value = (string)result11.Attribute("value") + " to " + (string)result12.Attribute("value"),
                            place1 = (string)result.Attribute("base_Class"),
                            place2 = (string)result4.Attribute("base_Activity"),
                            value_type = "$date$"
                        };

                foreach (Thing thing in results)
                {
                    tuple_types = tuple_types.Concat(new List<Thing>(){
                        new Thing
                        {
                            type = "activityPartOfProjectType",
                            id = thing.place2 + thing.type + "_apt1",
                            name = "$none$",
                            value = "$none$",
                            place1 = thing.type,
                            place2 = thing.place2,
                            foundation = "WholePartType",
                            value_type = "$none$"
                        }
                    });

                    //things = things.Concat(new List<Thing>(){
                    //    new Thing
                    //    {
                    //        type = "PeriodType",
                    //        id = thing.id + thing.type + "_t1",
                    //        name = (string)thing.value,
                    //        value = "$none$",
                    //        place1 = "$none$",
                    //        place2 = "$none$",
                    //        foundation = "IndividualType",
                    //        value_type = "$none$"
                    //    }
                    //});

                    //tuple_types = tuple_types.Concat(new List<Thing>(){
                    //    new Thing
                    //    {
                    //        type = "HappensInType",
                    //        id = thing.id + thing.type + "_t2",
                    //        name = "$none$",
                    //        value = (string)thing.value,
                    //        place1 = thing.id + thing.type + "_t1",
                    //        place2 = thing.id,
                    //        foundation = "WholePartType",
                    //        value_type = "$period$"
                    //    }
                    //});

                    //tuple_types = tuple_types.Concat(new List<Thing>(){
                    //    new Thing
                    //    {
                    //        type = "typeInstance",
                    //        id = thing.place1 + thing.type + "_ti1",
                    //        name = "$none$",
                    //        value = "$none$",
                    //        place1 = thing.type,
                    //        place2 = thing.place1,
                    //        foundation = "typeInstance",
                    //        value_type = "$none$"
                    //    }
                    //});

                    //tuple_types = tuple_types.Concat(new List<Thing>(){
                    //    new Thing
                    //    {
                    //        type = "activityPartOfCapability",
                    //        id = thing.place2 + thing.place1 + "_apc1",
                    //        name = "$none$",
                    //        value = "$none$",
                    //        place1 = thing.place1,
                    //        place2 = thing.place2,
                    //        foundation = "WholePartType",
                    //        value_type = "$none$"
                    //    }
                    //});

                    values.Add(new Thing { id = "_17", type = "CV-3", place2 = thing.place1, value = "Capability", place1 = "_17" });
                    values.Add(new Thing { id = "_17", type = "CV-3", place2 = thing.type, value = "ProjectType", place1 = "_17" });
                    values.Add(new Thing { id = "_17", type = "CV-3", place2 = thing.place2, value = "Activity", place1 = "_17" });
                    values.Add(new Thing { id = "_17", type = "CV-3", place2 = thing.id, value = "Project", place1 = "_17" });
                    //values.Add(new Thing { id = "_17", type = "CV-3", place2 = thing.id + thing.type + "_t1", value = "PeriodType", place1 = "_17" });
                    //values.Add(new Thing { id = "_14", type = "CV-3", place2 = thing.place2 + thing.place1 + "_apc1", value = "activityPartOfCapability", place1 = "_17" });
                    //values.Add(new Thing { id = "_14", type = "PV-2", place2 = thing.place1 + thing.type + "_ti1", value = "typeInstance", place1 = "_14" });
                    //values.Add(new Thing { id = "_14", type = "PV-2", place2 = thing.place2 + thing.type + "_apt1", value = "activityPartOfProjectType", place1 = "_14" });

                }

                results =
                    //from result in root.Elements(ns + "Capability")
                    //from result2 in root.Descendants().Elements("supplier")
                    //where (string)result.Attribute("base_Class") == (string)result2.Attribute(ns2 + "idref")
                    //from result3 in root.Descendants().Elements("client")
                    //where (string)result3.Parent.Attribute(ns2 + "id") == (string)result2.Parent.Attribute(ns2 + "id")
                    //from result4 in root.Elements(ns + "OperationalActivity")
                    //where (string)result4.Attribute("base_Activity") == (string)result3.Attribute(ns2 + "idref")
                    from result5 in root.Elements(ns + "Project")
                    //from result6 in root.Descendants().Elements("supplier")
                    //where (string)result5.Attribute("base_InstanceSpecification") == (string)result6.Attribute(ns2 + "idref")
                    //from result7 in root.Descendants().Elements("client")
                    //where (string)result6.Parent.Attribute(ns2 + "id") == (string)result7.Parent.Attribute(ns2 + "id")
                    //where (string)result4.Attribute("base_Activity") == (string)result7.Attribute(ns2 + "idref")

                    from result8 in root.Descendants().Elements("classifier")
                    from result9 in root.Elements(ns + "ProjectType")
                    where (string)result8.Attribute(ns2 + "idref") == (string)result9.Attribute("base_Class")
                    where (string)result8.Parent.Attribute(ns2 + "id") == (string)result5.Attribute("base_InstanceSpecification")


                    from result11 in root.Descendants()
                    where (string)result5.Attribute("startDate") != null
                    where (string)result5.Attribute("startDate") == (string)result11.Attribute(ns2 + "id")
                    from result12 in root.Descendants()
                    where (string)result5.Attribute("endDate") != null
                    where (string)result5.Attribute("endDate") == (string)result12.Attribute(ns2 + "id")


            select new Thing
            {
                type = (string)result9.Attribute("base_Class"),
                id = (string)result5.Attribute("base_InstanceSpecification"),
                name = "$none$",
                value = (string)result11.Attribute("value") + " to " + (string)result12.Attribute("value"),
                //place1 = (string)result.Attribute("base_Class"),
                //place2 = (string)result4.Attribute("base_Activity"),
                value_type = "$date$"
            };

                foreach (Thing thing in results)
                {
                    //tuple_types = tuple_types.Concat(new List<Thing>(){
                    //    new Thing
                    //    {
                    //        type = "activityPartOfProjectType",
                    //        id = thing.place2 + thing.type + "_apt1",
                    //        name = "$none$",
                    //        value = "$none$",
                    //        place1 = thing.type,
                    //        place2 = thing.place2,
                    //        foundation = "WholePartType",
                    //        value_type = "$none$"
                    //    }
                    //});

                    things = things.Concat(new List<Thing>(){
                        new Thing
                        {
                            type = "PeriodType",
                            id = thing.id + thing.type + "_t1",
                            name = (string)thing.value,
                            value = "$none$",
                            place1 = "$none$",
                            place2 = "$none$",
                            foundation = "IndividualType",
                            value_type = "$none$"
                        }
                    });

                    tuple_types = tuple_types.Concat(new List<Thing>(){
                        new Thing
                        {
                            type = "HappensInType",
                            id = thing.id + thing.type + "_t2",
                            name = "$none$",
                            value = (string)thing.value,
                            place1 = thing.id + thing.type + "_t1",
                            place2 = thing.id,
                            foundation = "WholePartType",
                            value_type = "$period$"
                        }
                    });

                    //tuple_types = tuple_types.Concat(new List<Thing>(){
                    //    new Thing
                    //    {
                    //        type = "typeInstance",
                    //        id = thing.place1 + thing.type + "_ti1",
                    //        name = "$none$",
                    //        value = "$none$",
                    //        place1 = thing.type,
                    //        place2 = thing.place1,
                    //        foundation = "typeInstance",
                    //        value_type = "$none$"
                    //    }
                    //});

                    //tuple_types = tuple_types.Concat(new List<Thing>(){
                    //    new Thing
                    //    {
                    //        type = "activityPartOfCapability",
                    //        id = thing.place2 + thing.place1 + "_apc1",
                    //        name = "$none$",
                    //        value = "$none$",
                    //        place1 = thing.place1,
                    //        place2 = thing.place2,
                    //        foundation = "WholePartType",
                    //        value_type = "$none$"
                    //    }
                    //});

                    //values.Add(new Thing { id = "_17", type = "CV-3", place2 = thing.place1, value = "Capability", place1 = "_17" });
                    //values.Add(new Thing { id = "_17", type = "CV-3", place2 = thing.type, value = "ProjectType", place1 = "_17" });
                    //values.Add(new Thing { id = "_17", type = "CV-3", place2 = thing.place2, value = "Activity", place1 = "_17" });
                    //values.Add(new Thing { id = "_17", type = "CV-3", place2 = thing.id, value = "Project", place1 = "_17" });
                    values.Add(new Thing { id = "_17", type = "CV-3", place2 = thing.id + thing.type + "_t1", value = "PeriodType", place1 = "_17" });
                    //values.Add(new Thing { id = "_14", type = "CV-3", place2 = thing.place2 + thing.place1 + "_apc1", value = "activityPartOfCapability", place1 = "_17" });
                    //values.Add(new Thing { id = "_14", type = "PV-2", place2 = thing.place1 + thing.type + "_ti1", value = "typeInstance", place1 = "_14" });
                    //values.Add(new Thing { id = "_14", type = "PV-2", place2 = thing.place2 + thing.type + "_apt1", value = "activityPartOfProjectType", place1 = "_14" });

                }

                sorted_results.Add(values);

                sorted_results_new = new List<List<Thing>>();
                Add_Tuples(ref sorted_results, ref sorted_results_new, tuples.ToList(), ref errors_list);
                Add_Tuples(ref sorted_results, ref sorted_results_new, tuple_types.ToList(), ref errors_list);
                sorted_results = sorted_results_new;

                foreach (Thing thing in sorted_results.First())
                {
                    if ((string)thing.value == "Activity" || (string)thing.value == "Capability" || (string)thing.value == "activityPartOfCapability"
                        || (string)thing.value == "ProjectType" || (string)thing.value == "activityPartOfProjectType" || (string)thing.value == "desiredResourceStateOfCapability")
                        mandatory_list.Add(new Thing { id = thing.place2, type = (string)thing.value, value = "$none$", value_type = "$none$" });
                    else
                        optional_list.Add(new Thing { id = thing.place2, type = (string)thing.value, value = "$none$", value_type = "$none$" });
                }

                if (sorted_results.First().Count() > 0)
                    views.Add(new View { type = "CV-3", id = "_17", name = "NEAR CV-3", optional = optional_list, mandatory = mandatory_list });

            ////SvcV-6

            //    mandatory_list = new List<Thing>();
            //    values = new List<Thing>();
            //    optional_list = new List<Thing>();
            //    sorted_results = new List<List<Thing>>();

            //    values_dic2 = things_dic.Where(x => x.Value.type == "Data").ToDictionary(p => p.Key, p => p.Value);

            //    results = tuple_types.Where(x => x.type == "activityConsumesResource").Where(x => values_dic2.ContainsKey(x.place1));
            //    values_dic = tuple_types.Where(x => x.type == "activityProducesResource").GroupBy(x => x.place2).Select(grp => grp.First()).ToDictionary(x => x.place2, x => x);

            //    foreach (Thing rela in results)
            //    {
            //        if (values_dic.TryGetValue(rela.place1, out value))
            //        {
            //            values.Add(rela);
            //            values.Add(value);
            //        }

            //    }

            //    count = 0;
            //    count2 = values.Count();

            //    //var duplicateKeys = app2.GroupBy(x => x.place2)
            //    //            .Where(group => group.Count() > 1)
            //    //            .Select(group => group.Key);

            //    //List<string> test = duplicateKeys.ToList();

            //    values_dic2 = tuple_types.Where(x => x.type == "activityPerformedByPerformer").Where(x => Allowed_Element("SvcV-6", x.place1, ref things_dic)).GroupBy(x => x.place2).Select(grp => grp.First()).ToDictionary(x => x.place2, x => x);

            //    while (count < count2)
            //    {
            //        add = false;

            //        foreach (Thing thing in values)
            //        {
            //            if (values_dic2.TryGetValue(values[count].place2, out value))
            //                if (values_dic2.TryGetValue(values[count + 1].place1, out value2))
            //                {
            //                    add = true;
            //                    values.Add(value);
            //                    values.Add(value2);
            //                    break;
            //                }
            //        }


            //        if (add == true)
            //        {
            //            count = count + 2;
            //        }
            //        else
            //        {
            //            values.RemoveAt(count);
            //            values.RemoveAt(count);
            //            count2 = count2 - 2;
            //        }
            //    }

            //    sorted_results.Add(Add_Places(things_dic, values));

            //    foreach (Thing thing in sorted_results.First())
            //    {

            //        temp = Find_Mandatory_Optional(thing.type, "SvcV-6", "SvcV-6", "_16", ref errors_list);
            //        if (temp == "Mandatory")
            //            mandatory_list.Add(new Thing { id = thing.id, type = thing.type, value = "$none$", value_type = "$none$" });
            //        if (temp == "Optional")
            //            optional_list.Add(new Thing { id = thing.id, type = thing.type, value = "$none$", value_type = "$none$" });

            //        if ((string)thing.type == "Service")
            //        {
            //            values = new List<Thing>();

            //            values.Add(new Thing
            //            {
            //                type = "ServiceDescription",
            //                id = thing.place2 + "_2",
            //                name = thing.place2 + "_Description",
            //                value = "$none$",
            //                place1 = "$none$",
            //                place2 = "$none$",
            //                foundation = "Individual",
            //                value_type = "$none$"
            //            });

            //            values.Add(new Thing
            //            {
            //                type = "serviceDescribedBy",
            //                id = thing.place2 + "_1",
            //                name = "$none$",
            //                value = "$none$",
            //                place1 = thing.id,
            //                place2 = thing.id + "_2",
            //                foundation = "namedBy",
            //                value_type = "$none$"
            //            });

            //            mandatory_list.AddRange(values);
            //        }
            //    }

            //    if (sorted_results.First().Count() > 0)
            //        views.Add(new View { type = "SvcV-6", id = "_16", name = "NEAR SvcV-6", optional = optional_list, mandatory = mandatory_list });

            //SV-8

            mandatory_list = new List<Thing>();
            values = new List<Thing>();
            optional_list = new List<Thing>();
            sorted_results = new List<List<Thing>>();

            //values_dic = things_dic.Where(x => x.Value.type == "Activity").ToDictionary(p => p.Key, p => p.Value);
            //values_dic2 = things_dic.Where(x => x.Value.type == "Capability").ToDictionary(p => p.Key, p => p.Value);

            //results = tuple_types.Where(x => x.type == "activityPartOfCapability").Where(x => values_dic2.ContainsKey(x.place1)).Where(x => values_dic.ContainsKey(x.place2));

            values_dic = things.Where(x => x.type == "System").ToDictionary(x=>x.id,x=>x);

            values2 = tuple_types.Where(x => x.type == "activityPerformedByPerformer").Where(x => values_dic.ContainsKey(x.place1)).ToList();//.GroupBy(x => x.place2).Select(grp => grp.First()).ToDictionary(x => x.place2, x => x);

            values_dic3 = tuple_types.Where(x => x.type == "HappensInType").ToDictionary(x => x.place2, x => x);

            foreach (Thing thing in values2)
            {
                if (values_dic3.TryGetValue(thing.place2, out value))
                {
                    //mandatory_list.Add(new Thing { id = thing.id, type = thing.type, value = "$none$", value_type = "$none$" });
                    values.Add(new Thing { id = "_14", type = "SV-8", place2 = thing.place1, value = "System", place1 = "_14" });
                    values.Add(new Thing { id = "_14", type = "SV-8", place2 = thing.place2, value = "Activity", place1 = "_14" });
                    values.Add(new Thing { id = "_14", type = "SV-8", place2 = value.place1, value = "PeriodType", place1 = "_14" });
                }
            }

            sorted_results.Add(values);

            sorted_results_new = new List<List<Thing>>();
            Add_Tuples(ref sorted_results, ref sorted_results_new, tuples.ToList(), ref errors_list);
            Add_Tuples(ref sorted_results, ref sorted_results_new, tuple_types.ToList(), ref errors_list);
            sorted_results = sorted_results_new;

            foreach (Thing thing in sorted_results.First())
            {
                if ((string)thing.value == "System")
                    mandatory_list.Add(new Thing { id = thing.place2, type = (string)thing.value, value = "$none$", value_type = "$none$" });
                else
                    optional_list.Add(new Thing { id = thing.place2, type = (string)thing.value, value = "$none$", value_type = "$none$" });
            }

            if (sorted_results.First().Count() > 0)
                views.Add(new View { type = "SV-8", id = "_14", name = "NEAR SV-8", optional = optional_list, mandatory = mandatory_list });

            //SV-5

            mandatory_list = new List<Thing>();
            values = new List<Thing>();
            values2 = new List<Thing>();
            optional_list = new List<Thing>();
            sorted_results = new List<List<Thing>>();

            results =
                       from result in root.Elements(ns + "Implements")
                       //from result3 in root.Elements(ns + "View")
                       //from result4 in root.Descendants()
                       from result2 in root.Descendants()
                       //from result2 in root.Elements(ns3 + "Package").Elements("packagedElement")
                       //where result2.Attribute("name") != null
                       where (string)result.LastAttribute == (string)result2.Attribute(ns2 + "id")
                       //where (string)result3.LastAttribute == (string)result4.Attribute(ns2 + "id")
                       select new Thing
                       {
                           type = "OverlapType",
                           id = (string)result.LastAttribute,
                           name = "$none$",
                           value = (string)result2.Element("supplier").Attribute(ns2 + "idref") + (string)result2.Element("client").Attribute(ns2 + "idref"),
                           place2 = (string)result2.Element("client").Attribute(ns2 + "idref"),
                           place1 = (string)result2.Element("supplier").Attribute(ns2 + "idref"),
                           foundation = "CoupleType",
                           value_type = "$id$"
                       };

            results_dic = tuple_types.Where(x => x.type == "activityPerformedByPerformer").GroupBy(x => x.place2).ToDictionary(x => x.Key, x => x.ToList());

            foreach (Thing thing in results)
            {
                if (results_dic.TryGetValue(thing.place2, out values2))
                {
                    foreach (Thing element in values2)
                    {
                        if(things_dic[element.place1].type == "System")
                            values.Add(new Thing { id = "_74", type = "SV-5a", place2 = element.place1, value = "System", place1 = "_74" });
                    }
                }
                values.Add(new Thing { id = "_74", type = "SV-5a", place2 = thing.place1, value = "Activity", place1 = "_74" });
                values.Add(new Thing { id = "_74", type = "SV-5a", place2 = thing.place2, value = "Activity", place1 = "_74" });  
            }

            sorted_results.Add(values);

            sorted_results_new = new List<List<Thing>>();
            Add_Tuples(ref sorted_results, ref sorted_results_new, tuples.ToList(), ref errors_list);
            Add_Tuples(ref sorted_results, ref sorted_results_new, tuple_types.ToList(), ref errors_list);
            sorted_results = sorted_results_new;

            foreach (Thing thing in sorted_results.First())
            {
                if ((string)thing.value == "System" || (string)thing.value == "Activity" || (string)thing.value == "activityPerformedByPerfomer")
                    mandatory_list.Add(new Thing { id = thing.place2, type = (string)thing.value, value = "$none$", value_type = "$none$" });
                else
                    optional_list.Add(new Thing { id = thing.place2, type = (string)thing.value, value = "$none$", value_type = "$none$" });
            }

            mandatory_list = mandatory_list.OrderBy(x => x.type).ToList();
            optional_list = optional_list.OrderBy(x => x.type).ToList();

            if (sorted_results.First().Count() > 0)
                views.Add(new View { type = "SV-5a", id = "_74", name = "NEAR SV-5a", optional = optional_list, mandatory = mandatory_list });


            //SvcV-5

            mandatory_list = new List<Thing>();
            values = new List<Thing>();
            values2 = new List<Thing>();
            optional_list = new List<Thing>();
            sorted_results = new List<List<Thing>>();

            results =
                       from result in root.Elements(ns + "Implements")
                       //from result3 in root.Elements(ns + "View")
                       //from result4 in root.Descendants()
                       from result2 in root.Descendants()
                       //from result2 in root.Elements(ns3 + "Package").Elements("packagedElement")
                       //where result2.Attribute("name") != null
                       where (string)result.LastAttribute == (string)result2.Attribute(ns2 + "id")
                       //where (string)result3.LastAttribute == (string)result4.Attribute(ns2 + "id")
                       select new Thing
                       {
                           type = "OverlapType",
                           id = (string)result.LastAttribute,
                           name = "$none$",
                           value = (string)result2.Element("supplier").Attribute(ns2 + "idref") + (string)result2.Element("client").Attribute(ns2 + "idref"),
                           place2 = (string)result2.Element("client").Attribute(ns2 + "idref"),
                           place1 = (string)result2.Element("supplier").Attribute(ns2 + "idref"),
                           foundation = "CoupleType",
                           value_type = "$id$"
                       };

            results_dic = tuple_types.Where(x => x.type == "activityPerformedByPerformer").GroupBy(x => x.place2).ToDictionary(x => x.Key, x => x.ToList());

            foreach (Thing thing in results)
            {
                if (results_dic.TryGetValue(thing.place2, out values2))
                {
                    foreach (Thing element in values2)
                    {
                        if (things_dic[element.place1].type == "Service")
                            values.Add(new Thing { id = "_75", type = "SvcV-5", place2 = element.place1, value = "Service", place1 = "_75" });
                    }
                }
                values.Add(new Thing { id = "_75", type = "SvcV-5", place2 = thing.place1, value = "Activity", place1 = "_75" });
                values.Add(new Thing { id = "_75", type = "SvcV-5", place2 = thing.place2, value = "Activity", place1 = "_75" });
            }

            sorted_results.Add(values);

            sorted_results_new = new List<List<Thing>>();
            Add_Tuples(ref sorted_results, ref sorted_results_new, tuples.ToList(), ref errors_list);
            Add_Tuples(ref sorted_results, ref sorted_results_new, tuple_types.ToList(), ref errors_list);
            sorted_results = sorted_results_new;

            foreach (Thing thing in sorted_results.First())
            {
                if ((string)thing.value == "Service" || (string)thing.value == "Activity" || (string)thing.value == "activityPerformedByPerfomer")
                    mandatory_list.Add(new Thing { id = thing.place2, type = (string)thing.value, value = "$none$", value_type = "$none$" });
                else
                    optional_list.Add(new Thing { id = thing.place2, type = (string)thing.value, value = "$none$", value_type = "$none$" });

                if ((string)thing.value == "Service")
                {
                    values = new List<Thing>();

                    values.Add(new Thing
                    {
                        type = "ServiceDescription",
                        id = thing.place2 + "_2",
                        name = thing.place2 + "_Description",
                        value = "$none$",
                        place1 = "$none$",
                        place2 = "$none$",
                        foundation = "Individual",
                        value_type = "$none$"
                    });

                    values.Add(new Thing
                    {
                        type = "serviceDescribedBy",
                        id = thing.place2 + "_1",
                        name = "$none$",
                        value = "$none$",
                        place1 = thing.id,
                        place2 = thing.id + "_2",
                        foundation = "namedBy",
                        value_type = "$none$"
                    });

                    mandatory_list.AddRange(values);

                    values = new List<Thing>();

                    values.Add(new Thing
                    {
                        type = "ServiceDescription",
                        id = thing.place2 + "_2",
                        name = thing.place2 + "_Description",
                        value = "$none$",
                        place1 = "$none$",
                        place2 = "$none$",
                        foundation = "Individual",
                        value_type = "$none$"
                    });

                    things = things.Concat(values);

                    values = new List<Thing>();

                    values.Add(new Thing
                    {
                        type = "serviceDescribedBy",
                        id = thing.place2 + "_1",
                        name = "$none$",
                        value = "$none$",
                        place1 = thing.id,
                        place2 = thing.id + "_2",
                        foundation = "namedBy",
                        value_type = "$none$"
                    });

                    tuples = tuples.Concat(values);
                }
            }

            mandatory_list = mandatory_list.OrderBy(x => x.type).ToList();
            optional_list = optional_list.OrderBy(x => x.type).ToList();

            if (sorted_results.First().Count() > 0)
                views.Add(new View { type = "SvcV-5", id = "_75", name = "NEAR SvcV-5", optional = optional_list, mandatory = mandatory_list });

            //Std-2

            mandatory_list = new List<Thing>();
            values = new List<Thing>();
            values2 = new List<Thing>();
            optional_list = new List<Thing>();
            sorted_results = new List<List<Thing>>();

            results_dic = tuple_types.Where(x => x.type == "activityPerformedByPerformer").GroupBy(x => x.place2).ToDictionary(x => x.Key, x => x.ToList());

            results =
                 from result in root.Elements(ns + "OperationalActivity")
                 where (string)result.Attribute("conformsTo") != null
                 from result2 in root.Descendants()
                 where (string)result.Attribute("conformsTo") == (string)result2.Attribute(ns2 + "id")
                 where (string)result2.Attribute("name") != null

                 select new Thing
                 {
                     type = "ruleConstrainsActivity",
                     id = (string)result.Attribute("base_Activity")+"_77",
                     name = (string)result2.Attribute("name"),
                     value = (string)result.Attribute("base_Activity"),
                     place1 = (string)result.Attribute("conformsTo"),
                     place2 = (string)result.Attribute("base_Activity"),
                     foundation = "CoupleType",
                     value_type = "$id$"
                 };

            tuple_types = tuple_types.Concat(results);

            values3 = new List<Thing>();
            foreach (Thing thing in results)
            {
                values3.Add(
                     new Thing
                     {
                         type = "Rule",
                         id = (string)thing.value + "_78",
                         name = thing.name,
                         value = "$none$",
                         place1 = "$none$",
                         place2 = "$none$",
                         foundation = "IndividualType",
                         value_type = "$none$"
                     }
                 );

                if (results_dic.TryGetValue(thing.place2, out values2))
                {
                    foreach (Thing element in values2)
                    {
                        values.Add(new Thing { id = "_84", type = "Std-2", place2 = element.place1, value = things_dic[element.place1].type, place1 = "_84" });
                    }
                }

                values.Add(new Thing { id = "_84", type = "Std-2", place2 = thing.place1, value = "Rule", place1 = "_84" });
                values.Add(new Thing { id = "_84", type = "Std-2", place2 = thing.place2, value = "Activity", place1 = "_84" });

            }

            things = things.Concat(values3);

            results =
                 from result in root.Elements(ns + "Function")
                 where (string)result.Attribute("conformsTo") != null
                 from result2 in root.Descendants()
                 where (string)result.Attribute("conformsTo") == (string)result2.Attribute(ns2 + "id")
                 where (string)result2.Attribute("name") != null

                 select new Thing
                 {
                     type = "ruleConstrainsActivity",
                     id = (string)result.Attribute("base_Activity") + "_77",
                     name = (string)result2.Attribute("name"),
                     value = (string)result.Attribute("base_Activity"),
                     place1 = (string)result.Attribute("conformsTo"),
                     place2 = (string)result.Attribute("base_Activity"),
                     foundation = "CoupleType",
                     value_type = "$id$"
                 };

            tuple_types = tuple_types.Concat(results);

            values3 = new List<Thing>();
            foreach (Thing thing in results)
            {
                values3.Add(
                     new Thing
                     {
                         type = "Rule",
                         id = (string)thing.value + "_78",
                         name = thing.name,
                         value = "$none$",
                         place1 = "$none$",
                         place2 = "$none$",
                         foundation = "IndividualType",
                         value_type = "$none$"
                     }
                 );

                if (results_dic.TryGetValue(thing.place2, out values2))
                {
                    foreach (Thing element in values2)
                    {
                        values.Add(new Thing { id = "_84", type = "Std-2", place2 = element.place1, value = things_dic[element.place1].type, place1 = "_84" });
                    }
                }

                values.Add(new Thing { id = "_84", type = "Std-2", place2 = thing.place1, value = "Rule", place1 = "_84" });
                values.Add(new Thing { id = "_84", type = "Std-2", place2 = thing.place2, value = "Activity", place1 = "_84" });

            }

            things = things.Concat(values3);

            sorted_results.Add(values);

            sorted_results_new = new List<List<Thing>>();
            Add_Tuples(ref sorted_results, ref sorted_results_new, tuples.ToList(), ref errors_list);
            Add_Tuples(ref sorted_results, ref sorted_results_new, tuple_types.ToList(), ref errors_list);
            sorted_results = sorted_results_new;

            foreach (Thing thing in sorted_results.First())
            {
                if ((string)thing.value == "Activity" || (string)thing.value == "ruleConstrainsActivity" || (string)thing.value == "activityPerformedByPerformer")
                    mandatory_list.Add(new Thing { id = thing.place2, type = (string)thing.value, value = "$none$", value_type = "$none$" });
                else
                    optional_list.Add(new Thing { id = thing.place2, type = (string)thing.value, value = "$none$", value_type = "$none$" });
            }

            mandatory_list = mandatory_list.OrderBy(x => x.type).ToList();
            optional_list = optional_list.OrderBy(x => x.type).ToList();

            if (sorted_results.First().Count() > 0)
                views.Add(new View { type = "Std-2", id = "_84", name = "NEAR Std-2", optional = optional_list, mandatory = mandatory_list });


            //BPMN BeforeAfter

            results =
                    from result3 in root.Descendants().Elements("edge")
                    from result2 in root.Descendants().Elements("node")
                    where (string)result2.Attribute("behavior") != null
                    where (string)result3.Attribute("source") == (string)result2.Attribute(ns2 + "id")
                    from result6 in root.Descendants().Elements("node")
                    where (string)result6.Attribute("behavior") != null
                    where (string)result3.Attribute("target") == (string)result6.Attribute(ns2 + "id")
                    from result in root.Elements(ns + "OperationalActivity")                    
                    where (string)result.Attribute("base_Activity") == (string)result2.Attribute("behavior")
                    from result5 in root.Elements(ns + "OperationalActivity")
                    where (string)result5.Attribute("base_Activity") == (string)result6.Attribute("behavior")             

                    select new Thing
                    {
                        type = "BeforeAfterType",
                        id = (string)result.Attribute("base_Activity") + (string)result5.Attribute("base_Activity") + "_1",// + "///" +*/ (string)result.LastAttribute,//Attribute("base_Operation"),
                        name = "$none$",
                        value = "$none$",
                        place1 = (string)result.Attribute("base_Activity"),
                        place2 = (string)result5.Attribute("base_Activity"),
                        value_type = "$none$",
                        foundation = "CoupleType"
                    };

            tuple_types = tuple_types.Concat(results);

            //Lookup

            results =
                    from result2 in root.Descendants().Elements("realizingActivityEdge")
                    from result3 in root.Descendants().Elements("conveyed")
                    where (string)result2.Parent.Attribute(ns2 + "id") == (string)result3.Parent.Attribute(ns2 + "id")

                    from result5 in root.Descendants().Elements("outgoing")
                    where (string)result5.Parent.Attribute("behavior") != null
                    where (string)result2.Attribute(ns2 + "idref") == (string)result5.Attribute(ns2 + "idref")

                    from result6 in root.Descendants().Elements("incoming")
                    where (string)result6.Parent.Attribute("behavior") != null
                    where (string)result2.Attribute(ns2 + "idref") == (string)result6.Attribute(ns2 + "idref")
                    select new Thing
                    {
                        type = "temp",
                        id = (string)result5.Parent.Attribute("behavior"),// + "///" +*/ (string)result.LastAttribute,//Attribute("base_Operation"),
                        value = "$none$",
                        place1 = (string)result2.Parent.Attribute(ns2 + "id"),
                        place2 = (string)result5.Parent.Attribute("behavior"),
                        value_type = "$none$"
                    };

            foreach (Thing thing in results)
            {

                if (lookup.TryGetValue(thing.place1, out values))
                {
                    values.Add(thing);
                    lookup.Remove(thing.place1);
                    lookup.Add(thing.place1, values);
                }
                else
                    lookup.Add(thing.place1, new List<Thing>() { thing });

            }

            results =
                from result2 in root.Descendants().Elements("realizingActivityEdge")
                from result3 in root.Descendants().Elements("conveyed")
                where (string)result2.Parent.Attribute(ns2 + "id") == (string)result3.Parent.Attribute(ns2 + "id")

                from result5 in root.Descendants().Elements("outgoing")
                where (string)result5.Parent.Attribute("behavior") != null
                where (string)result2.Attribute(ns2 + "idref") == (string)result5.Attribute(ns2 + "idref")

                from result6 in root.Descendants().Elements("incoming")
                where (string)result6.Parent.Attribute("behavior") != null
                where (string)result2.Attribute(ns2 + "idref") == (string)result6.Attribute(ns2 + "idref")
                select new Thing
                {
                    type = "temp",
                    id = (string)result6.Parent.Attribute("behavior"),// + "///" +*/ (string)result.LastAttribute,//Attribute("base_Operation"),
                    value = "$none$",
                    place1 = (string)result2.Parent.Attribute(ns2 + "id"),
                    place2 = (string)result6.Parent.Attribute("behavior"),
                    value_type = "$none$"
                };

            foreach (Thing thing in results)
            {

                if (lookup.TryGetValue(thing.place1, out values))
                {
                    values.Add(thing);
                    lookup.Remove(thing.place1);
                    lookup.Add(thing.place1, values);
                }
                else
                    lookup.Add(thing.place1, new List<Thing>() { thing });

            }

            results =
                    from result in root.Descendants().Elements("conveyed")
                    from result2 in root.Descendants().Elements("realizingActivityEdge")
                    where (string)result.Parent.Attribute(ns2 + "id") == (string)result2.Parent.Attribute(ns2 + "id")
                    select new Thing
                    {
                        type = "temp",
                        id = (string)result.Attribute(ns2 + "idref"),// + "///" +*/ (string)result.LastAttribute,//Attribute("base_Operation"),
                        value = "$none$",
                        place1 = (string)result2.Parent.Attribute(ns2 + "id"),
                        place2 = (string)result.Attribute(ns2 + "idref"),
                        value_type = "$none$"
                    };

            foreach (Thing thing in results)
            {

                if (lookup.TryGetValue(thing.place1, out values))
                {
                    values.Add(thing);
                    lookup.Remove(thing.place1);
                    lookup.Add(thing.place1, values);
                }
                else
                    lookup.Add(thing.place1, new List<Thing>() { thing });

            }

            results =
                    from result in root.Descendants().Elements("edge")
                    from result2 in root.Descendants().Elements("realizingActivityEdge")
                    where (string)result2.Attribute(ns2 + "idref") == (string)result.Attribute(ns2 + "id")
                    from result3 in root.Descendants().Elements("conveyed")
                    where (string)result2.Parent.Attribute(ns2 + "id") == (string)result3.Parent.Attribute(ns2 + "id")
                    select new Thing
                    {
                        type = "temp",
                        id = (string)result3.Attribute(ns2 + "idref"),// + "///" +*/ (string)result.LastAttribute,//Attribute("base_Operation"),
                        value = "$none$",
                        place1 = (string)result.Attribute(ns2 + "id"),
                        place2 = (string)result3.Attribute(ns2 + "idref"),
                        value_type = "$none$"
                    };

            foreach (Thing thing in results)
            {

                if (lookup.TryGetValue(thing.place1, out values))
                {
                    values.Add(thing);
                    lookup.Remove(thing.place1);
                    lookup.Add(thing.place1, values);
                }
                else
                    lookup.Add(thing.place1, new List<Thing>() { thing });

            }

            results =
                    from result in root.Elements(ns + "Performer")
                    //from result3 in root.Elements(ns + "View")
                    //from result4 in root.Descendants()
                    from result2 in root.Descendants()
                    //from result2 in root.Elements(ns3 + "Package").Elements("packagedElement")
                    //where result2.Attribute("name") != null
                    where (string)result.Attribute("base_Class") == (string)result2.Attribute("represents")
                    //where (string)result3.LastAttribute == (string)result4.Attribute(ns2 + "id")
                    select new Thing
                    {
                        type = "temp",
                        id = (string)result.Attribute("base_Class"),// + "///" +*/ (string)result.LastAttribute,//Attribute("base_Operation"),
                        value = "$none$",
                        place1 = (string)result2.Attribute(ns2 + "id"),
                        place2 = (string)result.Attribute("base_Class"),
                        value_type = "$none$"
                    };

            foreach (Thing thing in results)
            {
                lookup.Add(thing.place1, new List<Thing>(){thing});
            }

            results =
                    from result in root.Elements(ns + "OperationalActivity")
                    //from result3 in root.Elements(ns + "View")
                    //from result4 in root.Descendants()
                    from result2 in root.Descendants()
                    //from result2 in root.Elements(ns3 + "Package").Elements("packagedElement")
                    //where result2.Attribute("name") != null
                    where (string)result.Attribute("base_Activity") == (string)result2.Attribute("behavior")
                    //where (string)result3.LastAttribute == (string)result4.Attribute(ns2 + "id")
                    select new Thing
                    {
                        type = "temp",
                        id = (string)result.Attribute("base_Activity"),// + "///" +*/ (string)result.LastAttribute,//Attribute("base_Operation"),
                        value = "$none$",
                        place1 = (string)result2.Attribute(ns2 + "id"),
                        place2 = (string)result.Attribute("base_Activity"),
                        value_type = "$none$"
                    };

            foreach (Thing thing in results)
            {
                lookup.Add(thing.place1, new List<Thing>() { thing } );
            }

            results =
                   from result in root.Descendants().Elements("lifeline")
                   from result2 in root.Descendants().Elements("ownedAttribute")
                   where (string)result.Attribute("represents") == (string)result2.Attribute(ns2 + "id")
                   select new Thing
                   {
                       type = "temp",
                       id = (string)result2.Attribute("type"),// + "///" +*/ (string)result.LastAttribute,//Attribute("base_Operation"),
                       value = "$none$",
                       place1 = (string)result.Attribute(ns2 + "id"),
                       place2 = (string)result2.Attribute("type"),
                       value_type = "$none$"
                   };

            foreach (Thing thing in results)
            {
                lookup.Add(thing.place1, new List<Thing>() { thing });
            }

            results =
                   from result in root.Descendants().Elements("fragment")
                   from result2 in root.Descendants().Elements("message")
                   where (string)result.Attribute("message") == (string)result2.Attribute(ns2 + "id")
                   select new Thing
                   {
                       type = "temp",
                       id = (string)result.Attribute(ns2 + "id"),// + "///" +*/ (string)result.LastAttribute,//Attribute("base_Operation"),
                       value = "$none$",
                       place1 = (string)result.Attribute("message"),
                       place2 = (string)result.Attribute(ns2 + "id"),
                       value_type = "$none$"
                   };
            
            foreach (Thing thing in results)
            {

                if (lookup.TryGetValue(thing.place1, out values))
                {
                    values.Add(thing);
                    lookup.Remove(thing.place1);
                    lookup.Add(thing.place1, values);
                }
                else
                    lookup.Add(thing.place1, new List<Thing>() { thing });

            }

            results =
                    from result in root.Descendants().Elements("realization")
                    from result2 in root.Descendants().Elements("realizingActivityEdge")
                    where (string)result2.Parent.Attribute(ns2 + "id") != null
                    where (string)result.Parent.Attribute(ns2 + "id") != null
                    where (string)result.Parent.Attribute(ns2 + "id") == (string)result2.Parent.Attribute(ns2 + "id")
                    select new Thing
                    {
                        type = "temp",
                        id = (string)result2.Parent.Attribute(ns2 + "id"),// + "///" +*/ (string)result.LastAttribute,//Attribute("base_Operation"),
                        value = "$none$",
                        place1 = (string)result.Attribute(ns2 + "idref"),
                        place2 = (string)result2.Parent.Attribute(ns2 + "id"),
                        value_type = "$none$"
                    };

            foreach (Thing thing in results)
            {
                if (lookup.TryGetValue(thing.place2, out values))
                {
                    if (lookup.TryGetValue(thing.place1, out values2))
                    {
                        values.AddRange(values2);
                        lookup.Remove(thing.place1);
                        lookup.Add(thing.place1, values);
                    }
                    else
                    {
                        lookup.Add(thing.place1, values);
                    }
                }
            }

            tuple_types = tuple_types.GroupBy(x => x.id).Select(grp => grp.First());

            //Views

            foreach (string[] current_lookup in MD_View_Lookup)
            {
                results =
                    from result in root.Descendants().Elements("diagramContents").Elements("usedElements")
                    //from result3 in root.Elements(ns + "View")
                    //from result2 in root.Descendants()
                    //from result3 in root.Descendants()
                    //from result2 in root.Elements(ns3 + "Package").Elements("packagedElement")
                    //where result2.Attribute("name") != null
                    //where (string)result2.LastAttribute == (string)result.Attribute("element")
                    where (string)result.Parent.Parent.Attribute("type") == current_lookup[1]
                    select new Thing
                    {
                        type = current_lookup[0],
                        id = (string)result.Parent.Parent.Parent.Parent.Parent.Attribute(ns2 + "id") + (string)result.Value,// + "///" +*/ (string)result.LastAttribute,//Attribute("base_Operation"),
                        name = (string)result.Parent.Parent.Parent.Parent.Parent.Attribute("name"),
                        //value = Find_DM2_Type_RSA((string)result2.Name.LocalName.ToString()),
                        place1 = (string)result.Parent.Parent.Parent.Parent.Parent.Attribute(ns2 + "id"),
                        place2 = (string)result.Value,
                        foundation = "$none$",
                        value_type = "element_type"
                    };

                view_holder.Add(results.ToList());
            }

            foreach (List<Thing> view_elements in view_holder)
            {
                //foreach (Thing thing in values)
                int max = view_elements.Count;
                for (int i = 0; i < max; i++)
                {
                    Thing thing = view_elements[i];
                    //thing.value = (string) Find_Def_DM2_Type((string)thing.value, values5.ToList());

                    if (lookup.TryGetValue(thing.place2, out values))
                    {
                        count = 0;
                        foreach (Thing element in values)
                        {
                            if (count == 0)
                                thing.place2 = element.id;
                            if (count > 0)
                            {
                                view_elements.Add(new Thing
                                {
                                    type = thing.type,
                                    id = thing.id,
                                    name = thing.name,
                                    value = thing.value,
                                    place1 = thing.place1,
                                    place2 = element.id,
                                    foundation = thing.foundation,
                                    value_type = thing.value_type
                                });
                                max++;
                            }
                            count++;
                        }
                    }

                    if (thing.type == "DIV-2" || thing.type == "DIV-3")
                    if (div2_dic.TryGetValue(thing.place2, out values))
                    {
                        foreach (Thing additional in values)
                        {
                            view_elements.Add(new Thing
                            {
                                type = thing.type,
                                id = thing.id,
                                name = thing.name,
                                value = thing.value,
                                place1 = thing.place1,
                                place2 = additional.id,
                                foundation = thing.foundation,
                                value_type = thing.value_type
                            });
                            max++;
                        }
                        
                    }
                    if (thing.type == "DIV-3")
                    if (div3_dic.TryGetValue(thing.place2, out values))
                    {
                        foreach (Thing additional in values)
                        {
                            view_elements.Add(new Thing
                            {
                                type = thing.type,
                                id = thing.id,
                                name = thing.name,
                                value = thing.value,
                                place1 = thing.place1,
                                place2 = additional.id,
                                foundation = thing.foundation,
                                value_type = thing.value_type
                            });
                            max++;
                        }

                    }

                    if (thing.place2 != null)
                    {
                        if (things_dic.TryGetValue(thing.place2, out value))
                            thing.value = (string)value.type;
                    }
                }

                sorted_results = view_elements.GroupBy(x => x.place1).Select(group => group.Distinct().ToList()).ToList();
                //sorted_results = results.GroupBy(x => x.name).Select(group => group.Distinct().ToList()).ToList();

                sorted_results_new = new List<List<Thing>>();
                Add_Tuples(ref sorted_results, ref sorted_results_new, tuples.ToList(), ref errors_list);
                Add_Tuples(ref sorted_results, ref sorted_results_new, tuple_types.ToList(), ref errors_list);
                sorted_results = sorted_results_new;

                foreach (List<Thing> view in sorted_results)
                {

                    mandatory_list = new List<Thing>();
                    optional_list = new List<Thing>();

                    foreach (Thing thing in view)
                    {
                        if (thing.place2 != null)
                        {
                            temp = Find_Mandatory_Optional((string)thing.value, view.First().name, thing.type, thing.place1, ref errors_list);
                            if (temp == "Mandatory")
                            {
                                mandatory_list.Add(new Thing { id = thing.place2, type = (string)thing.value });
                            }
                            if (temp == "Optional")
                            {
                                optional_list.Add(new Thing { id = thing.place2, type = (string)thing.value });
                            }

                            values = new List<Thing>();
                            //if (sv8_dic.TryGetValue(thing.place2, out values))
                            //    optional_list.AddRange(values);


                                //if (div2_dic.TryGetValue(thing.place2, out values))
                                //    mandatory_list.AddRange(values);

                                //if (div2_dic2.TryGetValue(thing.place2, out values))
                                //    optional_list.AddRange(values);

                            //if (view.First().type == "DIV-3")
                           // {
                                //if (div3_dic.TryGetValue(thing.place2, out values))
                                //    mandatory_list.AddRange(values);

                             //   if (div3_dic2.TryGetValue(thing.place2, out values))
                             //       optional_list.AddRange(values);
                            //}

                            if (description_views.TryGetValue(thing.place2, out values))
                                optional_list.AddRange(values);

                            if (thing.type.Contains("SvcV"))
                            {
                                if ((string)thing.value == "Service")
                                {
                                    values = new List<Thing>();

                                    values.Add(new Thing
                                    {
                                        type = "ServiceDescription",
                                        id = thing.place2 + "_2",
                                        name = thing.place2 + "_Description",
                                        value = "$none$",
                                        place1 = "$none$",
                                        place2 = "$none$",
                                        foundation = "Individual",
                                        value_type = "$none$"
                                    });

                                    values.Add(new Thing
                                    {
                                        type = "serviceDescribedBy",
                                        id = thing.place2 + "_1",
                                        name = "$none$",
                                        value = "$none$",
                                        place1 = thing.id,
                                        place2 = thing.id + "_2",
                                        foundation = "namedBy",
                                        value_type = "$none$"
                                    });

                                    mandatory_list.AddRange(values);

                                    values = new List<Thing>();

                                    values.Add(new Thing
                                    {
                                        type = "ServiceDescription",
                                        id = thing.place2 + "_2",
                                        name = thing.place2 + "_Description",
                                        value = "$none$",
                                        place1 = "$none$",
                                        place2 = "$none$",
                                        foundation = "Individual",
                                        value_type = "$none$"
                                    });

                                    things = things.Concat(values);

                                    values = new List<Thing>();

                                    values.Add(new Thing
                                    {
                                        type = "serviceDescribedBy",
                                        id = thing.place2 + "_1",
                                        name = "$none$",
                                        value = "$none$",
                                        place1 = thing.id,
                                        place2 = thing.id + "_2",
                                        foundation = "namedBy",
                                        value_type = "$none$"
                                    });

                                    tuples = tuples.Concat(values);
                                }
                            }
                        }
                    }

                    values = new List<Thing>();

                    if (OV1_pic_views.TryGetValue(view.First().place1, out values))
                        mandatory_list.AddRange(values);

                    if (view.First().type == "SV-4" || view.First().type == "SvcV-4")
                    {
                        //var duplicateKeys = tuple_types.GroupBy(x => x.id)
                        //.Where(group => group.Count() > 1)
                        //.Select(group => group.Key);

                        values_dic4 = tuple_types.Where(x => x.type == "activityPerformedByPerformer").ToDictionary(x=>x.id,x=>x);

                        //values2 = mandatory_list.Where(x => x.type == "activityPerformedByPerformer").ToDictionary(x => x.id, x => values_dic4[x.id]).Values.ToList();

                        values2 = mandatory_list.Where(x => x.type == "activityPerformedByPerformer").Select(x => values_dic4[x.id]).ToList();

                        values_dic = values2.GroupBy(x => x.place2).Select(grp => grp.First()).ToDictionary(x => x.place2, x => x);

                        results = tuple_types.Where(x => x.type == "activityConsumesResource").Where(x => values_dic.ContainsKey(x.place2));

                        values_dic3 = things.Where(x => x.type == "Data").ToDictionary(x => x.id, x => x);

                        results = results.Where(x => values_dic3.ContainsKey(x.place1));

                        values_dic2 = results.GroupBy(x => x.place1).Select(grp => grp.First()).ToDictionary(x => x.place1, x => x);

                        values = tuple_types.Where(x => x.type == "activityProducesResource").Where(x => values_dic.ContainsKey(x.place1)).Where(x => values_dic2.ContainsKey(x.place2)).ToList();

                        mandatory_list.AddRange(results);

                        mandatory_list.AddRange(values);

                        //mandatory_list.AddRange(results.GroupBy(x => x.place1).Select(grp => grp.First()).ToDictionary(x => x.id, x => values_dic3[x.place1]).Values.ToList());

                        mandatory_list.AddRange(results.GroupBy(x => x.place1).Select(grp => grp.First()).Select( x => values_dic3[x.place1]));
                    }

                    mandatory_list = mandatory_list.OrderBy(x => x.type).ToList();
                    optional_list = optional_list.OrderBy(x => x.type).ToList();

                    if (Proper_View(mandatory_list, view.First().name, view.First().type, view.First().place1, ref errors_list))
                        views.Add(new View { type = view.First().type, id = view.First().place1, name = view.First().name, mandatory = mandatory_list, optional = optional_list });
                }
            }

            using (var sw = new Utf8StringWriter())
            {
                using (var writer = XmlWriter.Create(sw))
                {

                    writer.WriteRaw(@"<IdeasEnvelope OriginatingNationISO3166TwoLetterCode=""String"" ism:ownerProducer=""NMTOKEN"" ism:classification=""U""
                    xsi:schemaLocation=""http://cio.defense.gov/xsd/dm2 DM2_PES_v2.02_Chg_1.XSD""
                    xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:ism=""urn:us:gov:ic:ism:v2"" xmlns:ideas=""http://www.ideasgroup.org/xsd""
                    xmlns:dm2=""http://www.ideasgroup.org/dm2""><IdeasData XMLTagsBoundToNamingScheme=""DM2Names"" ontologyVersion=""2.02_Chg_1"" ontology=""DM2"">
		            <NamingScheme ideas:FoundationCategory=""NamingScheme"" id=""ns1""><ideas:Name namingScheme=""ns1"" id=""NamingScheme"" exemplarText=""DM2Names""/>
		            </NamingScheme>");
                    if (representation_scheme)
                        writer.WriteRaw(@"<RepresentationScheme ideas:FoundationCategory=""Type"" id=""id_rs1"">
			            <ideas:Name id=""RepresentationScheme"" namingScheme=""ns1"" exemplarText=""Base64 Encoded Image""/>
		                </RepresentationScheme>");

                    foreach (Thing thing in things)
                        writer.WriteRaw("<" + thing.type + " ideas:FoundationCategory=\"" + thing.foundation + "\" id=\"id" + thing.id + "\" "
                            + (((string)thing.value == "$none$") ? "" : thing.value_type + "=\"" + (string)thing.value + "\"") + ">" + "<ideas:Name exemplarText=\"" + thing.name
                            + "\" namingScheme=\"ns1\" id=\"n" + thing.id + "\"/></" + thing.type + ">");

                    foreach (Thing thing in tuple_types)
                        writer.WriteRaw("<" + thing.type + " ideas:FoundationCategory=\"" + thing.foundation + "\" id=\"id" + thing.id
                        + "\" place1Type=\"id" + thing.place1 + "\" place2Type=\"id" + thing.place2 + "\"/>");

                    foreach (Thing thing in tuples)
                        writer.WriteRaw("<" + thing.type + " ideas:FoundationCategory=\"" + thing.foundation + "\" id=\"id" + thing.id
                        + "\" tuplePlace1=\"id" + thing.place1 + "\" tuplePlace2=\"id" + thing.place2 + "\"/>");

                    writer.WriteRaw(@"</IdeasData>");

                    writer.WriteRaw(@"<IdeasViews frameworkVersion=""DM2.02_Chg_1"" framework=""DoDAF"">");

                    foreach (View view in views)
                    {
                        writer.WriteRaw("<" + view.type + " id=\"id" + view.id + "\" name=\"" + view.name + "\">");

                        writer.WriteRaw(@"<MandatoryElements>");

                        foreach (Thing thing in view.mandatory)
                        {
                            writer.WriteRaw("<" + view.type + "_" + thing.type + " ref=\"id" + thing.id + "\"/>");
                        }

                        writer.WriteRaw(@"</MandatoryElements>");
                        writer.WriteRaw(@"<OptionalElements>");

                        foreach (Thing thing in view.optional)
                        {
                            writer.WriteRaw("<" + view.type + "_" + thing.type + " ref=\"id" + thing.id + "\"/>");
                        }

                        writer.WriteRaw(@"</OptionalElements>");
                        writer.WriteRaw("</" + view.type + ">");
                    }

                    writer.WriteRaw(@"</IdeasViews>");

                    writer.WriteRaw(@"</IdeasEnvelope>");

                    writer.Flush();
                }

                output = sw.ToString();
                errors = string.Join("", errors_list.Distinct().ToArray());

                if (errors.Count() > 0)
                    test = false;

                return test;
            }
        }

        ////////////////////
        ////////////////////

        public static string PES2MD_FUTURE(byte[] input)
        {
            Dictionary<string, Thing> things = new Dictionary<string, Thing>();
            Dictionary<string, Thing> results_dic;
            Dictionary<string, Thing> OV1_pic_views = new Dictionary<string, Thing>();
            IEnumerable<Thing> tuple_types = new List<Thing>();
            IEnumerable<Thing> tuples = new List<Thing>();
            IEnumerable<Thing> results;
            List<View> views = new List<View>();
            string temp="";
            string temp2="";
            string temp3="";
            string date = DateTime.Now.ToString("d");
            string time = DateTime.Now.ToString("T");
            string prop_date = DateTime.Now.ToString("yyyyMMdd");
            string prop_time = DateTime.Now.ToString("HHmmss");
            string minor_type;
            Guid view_GUID;
            string thing_GUID;
            string thing_GUID_1;
            string thing_GUID_2;
            string thing_GUID_3;
            Dictionary<string, string> thing_GUIDs = new Dictionary<string, string>();
            List<string> MD_Def_elements = new List<string>();
            XElement root = XElement.Load(new MemoryStream(input));
            List<List<Thing>> sorted_results;
            //bool representation_scheme = false;
            int count = 0;
            int count2 = 0;
            Thing value;
            //List<Thing> values;
            XNamespace ns = "http://www.ideasgroup.org/xsd";
            Dictionary<string, Location> location_dic = new Dictionary<string, Location>();
            string loc_x, loc_y, size_x, size_y;
            Location location;
            List<string> errors_list = new List<string>();

            foreach (string[] current_lookup in RSA_Element_Lookup)
            {
                results_dic =
                    (from result in root.Elements("IdeasData").Descendants().Elements(ns + "Name")
                        where (string)result.Parent.Name.ToString() == current_lookup[0]
                        select new
                        {
                            key = ((string)result.Parent.Attribute("id")).Substring(2),
                            value = new Thing
                            {
                                type = current_lookup[0],
                                id = ((string)result.Parent.Attribute("id")).Substring(2),
                                name = (string)result.Attribute("exemplarText"),
                                value = current_lookup[1],
                                place1 = "$none$",
                                place2 = "$none$",
                                foundation = (string)result.Parent.Attribute(ns + "FoundationCategory"),
                                value_type = "MDObjMinorTypeName"
                            }
                        }).ToDictionary(a => a.key, a => a.value);


                if (results_dic.Count() > 0)
                    MergeDictionaries(things, results_dic);
            }

            // OV-1 Pic

            OV1_pic_views =
                   (
                    from result2 in root.Elements("IdeasViews").Descendants().Descendants().Descendants()
                    where (string)result2.Name.ToString() == "OV-1_ArchitecturalDescription"
                    from result in root.Elements("IdeasData").Descendants().Elements(ns + "Name")
                    where ((string)result2.Attribute("ref")).Substring(2) == ((string)result.Parent.Attribute("id")).Substring(2)
                    from result3 in root.Elements("IdeasData").Elements("representationSchemeInstance")
                    //where (string)result.Parent.Name.ToString() == "ArchitecturalDescription"
                    where ((string)result3.Attribute("tuplePlace2")).Substring(2) == ((string)result.Parent.Attribute("id")).Substring(2)
                    select new
                    {
                        key = ((string)result2.Parent.Parent.Attribute("id")).Substring(2),
                        value = new Thing
                        {
                            type = "ArchitecturalDescription",
                            id = ((string)result.Parent.Attribute("id")).Substring(2),
                            name = (string)result.Attribute("exemplarText"),
                            value = ((string)result.Parent.Attribute("exemplar")),
                            place1 = "$none$",
                            place2 = "$none$",
                            foundation = (string)result.Parent.Attribute(ns + "FoundationCategory"),
                            value_type = "Picture"
                        }
                    }).ToDictionary(a => a.key, a => a.value);

            if (OV1_pic_views.Count() > 0)
            {
                foreach (Thing thing in OV1_pic_views.Values.ToList())
                {
                    things.Remove(thing.id);
                }
            }

            //  diagramming

            results =
                     from result in root.Elements("IdeasData").Elements("SpatialMeasure").Elements(ns + "Name")
                     select new Thing
                     {
                         id = ((string)result.Parent.Attribute("id")).Substring(2, ((string)result.Parent.Attribute("id")).Length - 5),
                         name = (string)result.Attribute("exemplarText"),
                         value = (string)result.Parent.Attribute("numericValue"),
                         place1 = "$none$",
                         place2 = "$none$",
                         foundation = "$none$",
                         value_type = "diagramming"
                     };

            sorted_results = results.GroupBy(x => x.id).Select(group => group.OrderBy(x => x.name).ToList()).ToList();

            foreach (List<Thing> coords in sorted_results)
            {
                location_dic.Add(coords.First().id,
                    new Location
                    {
                        id = coords.First().id,
                        bottom_right_x = (string)coords[0].value,
                        bottom_right_y = (string)coords[1].value,
                        bottom_right_z = "0",
                        top_left_x = (string)coords[3].value,
                        top_left_y = (string)coords[4].value,
                        top_left_z = "0",
                    });
            }

            // regular tuples

            foreach (string[] current_lookup in Tuple_Lookup)
            {
                if (current_lookup[3] != "1" && current_lookup[3] != "5")
                    continue;

                results =
                    from result in root.Elements("IdeasData").Descendants()
                    where (string)result.Name.ToString() == current_lookup[0]
                    from result2 in root.Elements("IdeasData").Descendants()
                    where ((string)result.Attribute("tuplePlace1")) == ((string)result2.Attribute("id"))
                    where (string)result2.Name.ToString() == current_lookup[5]
                    select new Thing
                    {
                        type = current_lookup[0],
                        id = ((string)result.Attribute("id")).Substring(2),
                        name = "$none$",
                        value = (string)result2.Name.ToString(),
                        place1 = ((string)result.Attribute("tuplePlace1")).Substring(2),
                        place2 = ((string)result.Attribute("tuplePlace2")).Substring(2),
                        foundation = current_lookup[2],
                        value_type = "element type"
                    };

                tuples = tuples.Concat(results.ToList());
            }

            // regular tuple types

            foreach (string[] current_lookup in Tuple_Type_Lookup)
            {

                if (current_lookup[3] != "1" && current_lookup[3] != "5")
                    continue;

                results =
                    from result in root.Elements("IdeasData").Descendants()
                    where (string)result.Name.ToString() == current_lookup[0]
                    from result2 in root.Elements("IdeasData").Descendants()
                    where ((string)result.Attribute("place1Type")) == ((string)result2.Attribute("id"))
                    where (string)result2.Name.ToString() == current_lookup[5]

                    select new Thing
                    {
                        type = current_lookup[0],
                        id = ((string)result.Attribute("id")).Substring(2),
                        name = "$none$",
                        value = (string)result2.Name.ToString(),
                        place1 = ((string)result.Attribute("place1Type")).Substring(2),
                        place2 = ((string)result.Attribute("place2Type")).Substring(2),
                        foundation = current_lookup[2],
                        value_type = "element type"
                    };

                tuple_types = tuple_types.Concat(results.ToList());
            }

            // views

            foreach (string[] current_lookup in View_Lookup)
            {
                if (current_lookup[3] != "default")
                    continue;
                results =
                    from result in root.Elements("IdeasViews").Descendants().Descendants().Descendants()
                    where (string)result.Parent.Parent.Name.ToString() == current_lookup[0]
                    select new Thing
                    {
                        type = current_lookup[0],
                        id = ((string)result.Parent.Parent.Attribute("id")).Substring(2) + ((string)result.Attribute("ref")).Substring(2),
                        name = ((string)result.Parent.Parent.Attribute("name")).Replace("&", " And "),
                        place1 = ((string)result.Parent.Parent.Attribute("id")).Substring(2),
                        place2 = ((string)result.Attribute("ref")).Substring(2),
                        value = (things.TryGetValue(((string)result.Attribute("ref")).Substring(2), out value)) ? value : new Thing { type = "$none$" },
                        foundation = "$none$",
                        value_type = "Thing"
                    };


                sorted_results = results.GroupBy(x => x.name).Select(group => group.Distinct().ToList()).ToList();
                //sorted_results = Add_Tuples(sorted_results, tuples);
                //sorted_results = Add_Tuples(sorted_results, tuple_types);

                foreach (List<Thing> view in sorted_results)
                {
                    List<Thing> mandatory_list = new List<Thing>();
                    List<Thing> optional_list = new List<Thing>();

                    foreach (Thing thing in view)
                    {

                        temp = Find_Mandatory_Optional((string)((Thing)thing.value).type, view.First().name, thing.type, thing.place1, ref errors_list);
                        if (temp == "Mandatory")
                        {
                            mandatory_list.Add(new Thing { id = thing.place2, name = (string)((Thing)thing.value).name, type = (string)((Thing)thing.value).value });
                        }
                        if (temp == "Optional")
                        {
                            optional_list.Add(new Thing { id = thing.place2, name = (string)((Thing)thing.value).name, type = (string)((Thing)thing.value).value });
                        }
                    }

                    mandatory_list = mandatory_list.OrderBy(x => x.type).ToList();
                    optional_list = optional_list.OrderBy(x => x.type).ToList();

                    //if (needline_views.TryGetValue(view.First().place1, out values))
                    //    optional_list.AddRange(values);

                    //if (Proper_View(mandatory_list, view.First().type))
                    views.Add(new View { type = current_lookup[1], id = view.First().place1, name = view.First().name, mandatory = mandatory_list, optional = optional_list });
                }
            }

            foreach (string thing in things.Keys)
            {
                thing_GUID = "_" + Guid.NewGuid().ToString("N").Substring(10);
                
                thing_GUID_3 = thing_GUID.Substring(7, 16);

                thing_GUIDs.Add(thing, thing_GUID_3);
            }

            //  output

            using (var sw = new Utf8StringWriter())
            {
                using (var writer = XmlWriter.Create(sw))
                {

                    writer.WriteRaw(@"<xmi:XMI xmi:version=""2.0"" xmlns:xmi=""http://www.omg.org/XMI"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:SoaML=""http:///schemas/SoaML/_LLF3UPc5EeGmUaPBBKwKBw/136"" xmlns:UPIA=""http:///schemas/UPIA/_7hv4kEc6Ed-f1uPQXF_0HA/563"" xmlns:ecore=""http://www.eclipse.org/emf/2002/Ecore"" xmlns:notation=""http://www.eclipse.org/gmf/runtime/1.0.2/notation"" xmlns:uml=""http://www.eclipse.org/uml2/3.0.0/UML"" xmlns:umlnotation=""http://www.ibm.com/xtools/1.5.3/Umlnotation"" xsi:schemaLocation=""http:///schemas/SoaML/_LLF3UPc5EeGmUaPBBKwKBw/136 pathmap://SOAML/SoaML.epx#_LLGeYPc5EeGmUaPBBKwKBw?SoaML/SoaML? http:///schemas/UPIA/_7hv4kEc6Ed-f1uPQXF_0HA/563 pathmap://UPIA_HOME/UPIA.epx#_7im0MEc6Ed-f1uPQXF_0HA?UPIA/UPIA?"">
                        <uml:Model xmi:id=""_9R-2X9PyEeSa1bJT-ij9YA"" name=""UPIA Model"" viewpoint="""">
                        <eAnnotations xmi:id=""_9R-2YNPyEeSa1bJT-ij9YA"" source=""uml2.diagrams""/>
                        <eAnnotations xmi:id=""_9R-2YdPyEeSa1bJT-ij9YA"" source=""com.ibm.xtools.common.ui.reduction.editingCapabilities"">
                          <details xmi:id=""_9R-2YtPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBFragment"" value=""1""/>
                          <details xmi:id=""_9R-2Y9PyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBArtifact"" value=""1""/>
                          <details xmi:id=""_9R-2ZNPyEeSa1bJT-ij9YA"" key=""updm.project.activity"" value=""1""/>
                          <details xmi:id=""_9R-2ZdPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBFunction"" value=""1""/>
                          <details xmi:id=""_9R-2ZtPyEeSa1bJT-ij9YA"" key=""updm.standard.activity"" value=""1""/>
                          <details xmi:id=""_9R-2Z9PyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBStructureDiagram"" value=""1""/>
                          <details xmi:id=""_9R-2aNPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBSubsystem"" value=""1""/>
                          <details xmi:id=""_9R-2adPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBClass"" value=""1""/>
                          <details xmi:id=""_9R-2atPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBRelationship1"" value=""1""/>
                          <details xmi:id=""_9R-2a9PyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBRelationship2"" value=""1""/>
                          <details xmi:id=""_9R-2bNPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBStateMachine1"" value=""1""/>
                          <details xmi:id=""_9R-2bdPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBStateMachine2"" value=""1""/>
                          <details xmi:id=""_9R-2btPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBComponent"" value=""1""/>
                          <details xmi:id=""_9R-2b9PyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBDeploymentSpecification"" value=""1""/>
                          <details xmi:id=""_9R-2cNPyEeSa1bJT-ij9YA"" key=""updm.strategic.activity"" value=""1""/>
                          <details xmi:id=""_9R-2cdPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBAbstractionRelation"" value=""1""/>
                          <details xmi:id=""_9R-2ctPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBActivity1"" value=""1""/>
                          <details xmi:id=""_9R-2c9PyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBActivity2"" value=""1""/>
                          <details xmi:id=""_9R-2dNPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBAction"" value=""1""/>
                          <details xmi:id=""_9R-2ddPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBActivityDiagram"" value=""1""/>
                          <details xmi:id=""_9R-2dtPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBActivity3"" value=""1""/>
                          <details xmi:id=""_9R-2d9PyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBPackage"" value=""1""/>
                          <details xmi:id=""_9R-2eNPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBSequence1"" value=""1""/>
                          <details xmi:id=""_9R-2edPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBSequence2"" value=""1""/>
                          <details xmi:id=""_9R-2etPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBSequenceDiagram"" value=""1""/>
                          <details xmi:id=""_9R-2e9PyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBDependancy"" value=""1""/>
                          <details xmi:id=""_9R-2fNPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBLifeLine"" value=""1""/>
                          <details xmi:id=""_9R-2fdPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBUsage"" value=""1""/>
                          <details xmi:id=""_9R-2ftPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBFreeFormDiagram"" value=""1""/>
                          <details xmi:id=""_9R-2f9PyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBInstance"" value=""1""/>
                          <details xmi:id=""_9R-2gNPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBComponentDiagram"" value=""1""/>
                          <details xmi:id=""_9R-2gdPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBEvent1"" value=""1""/>
                          <details xmi:id=""_9R-2gtPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBEvent2"" value=""1""/>
                          <details xmi:id=""_9R-2g9PyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBTypes2"" value=""1""/>
                          <details xmi:id=""_9R-2hNPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBTypes4"" value=""1""/>
                          <details xmi:id=""_9R-2hdPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBCommunicationDiagram"" value=""1""/>
                          <details xmi:id=""_9R-2htPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBConstraint"" value=""1""/>
                          <details xmi:id=""_9R-2h9PyEeSa1bJT-ij9YA"" key=""updm.organizational.activity"" value=""1""/>
                          <details xmi:id=""_9R-2iNPyEeSa1bJT-ij9YA"" key=""updm.performance.activity"" value=""1""/>
                          <details xmi:id=""_9R-2idPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBInterface"" value=""1""/>
                          <details xmi:id=""_9R-2itPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBInformationFlow"" value=""1""/>
                          <details xmi:id=""_9R-2i9PyEeSa1bJT-ij9YA"" key=""updm.system.activity"" value=""1""/>
                          <details xmi:id=""_9R-2jNPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBComment1"" value=""1""/>
                          <details xmi:id=""_9R-2jdPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBComment2"" value=""1""/>
                          <details xmi:id=""_9R-2jtPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBCompositeStructure1"" value=""1""/>
                          <details xmi:id=""_9R-2j9PyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBCollaboration"" value=""1""/>
                          <details xmi:id=""_9R-2kNPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBRealization"" value=""1""/>
                          <details xmi:id=""_9R-2kdPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBCompositeStructure2"" value=""1""/>
                          <details xmi:id=""_9R-2ktPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBStateChartDiagram"" value=""1""/>
                          <details xmi:id=""_9R-2k9PyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBUseCase1"" value=""1""/>
                          <details xmi:id=""_9R-2lNPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBClassDiagram"" value=""1""/>
                          <details xmi:id=""_9R-2ldPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBUseCase2"" value=""1""/>
                          <details xmi:id=""_9R-2ltPyEeSa1bJT-ij9YA"" key=""updm.enterprise.activity"" value=""1""/>
                          <details xmi:id=""_9R-2l9PyEeSa1bJT-ij9YA"" key=""updm.service.activity"" value=""1""/>
                          <details xmi:id=""_9R-2mNPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBUseCaseDiagram"" value=""1""/>
                          <details xmi:id=""_9R-2mdPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBDeployment1"" value=""1""/>
                          <details xmi:id=""_9R-2mtPyEeSa1bJT-ij9YA"" key=""updm.operational.activity"" value=""1""/>
                          <details xmi:id=""_9R-2m9PyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBDeployment2"" value=""1""/>
                          <details xmi:id=""_9R-2nNPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBDeploymentDiagram"" value=""1""/>
                          <details xmi:id=""_9R-2ndPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBInteraction"" value=""1""/>
                          <details xmi:id=""_9R-2ntPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBCommunication"" value=""1""/>
                          <details xmi:id=""_9R-2n9PyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.mq"" value=""1""/>
                          <details xmi:id=""_9R-2oNPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.ldap"" value=""1""/>
                          <details xmi:id=""_9R-2odPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.j2ee"" value=""1""/>
                          <details xmi:id=""_9R-2otPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBPrimitiveTypeTemplateParameter"" value=""1""/>
                          <details xmi:id=""_9R-2o9PyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.analysisAndDesign.zephyrUML"" value=""1""/>
                          <details xmi:id=""_9R-2pNPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBElementImport1"" value=""1""/>
                          <details xmi:id=""_9R-2pdPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBElementImport2"" value=""1""/>
                          <details xmi:id=""_9R-2ptPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.db2"" value=""1""/>
                          <details xmi:id=""_9R-2p9PyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.mq"" value=""1""/>
                          <details xmi:id=""_9R-2qNPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBInterfaceTemplateParameter"" value=""1""/>
                          <details xmi:id=""_9R-2qdPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.was"" value=""1""/>
                          <details xmi:id=""_9R-2qtPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.ldap"" value=""1""/>
                          <details xmi:id=""_9R-2q9PyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.dotnet"" value=""1""/>
                          <details xmi:id=""_9R-2rNPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.j2ee"" value=""1""/>
                          <details xmi:id=""_9R-2rdPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBProfile"" value=""1""/>
                          <details xmi:id=""_9R-2rtPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.storage"" value=""1""/>
                          <details xmi:id=""_9R-2r9PyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.waswebplugin"" value=""1""/>
                          <details xmi:id=""_9R-2sNPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.dotnet"" value=""1""/>
                          <details xmi:id=""_9R-2sdPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.portlet"" value=""1""/>
                          <details xmi:id=""_9R-2stPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBSignal"" value=""1""/>
                          <details xmi:id=""_9R-2s9PyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.waswebplugin"" value=""1""/>
                          <details xmi:id=""_9R-2tNPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.messaging"" value=""1""/>
                          <details xmi:id=""_9R-2tdPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.jms"" value=""1""/>
                          <details xmi:id=""_9R-2ttPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.sqlserver"" value=""1""/>
                          <details xmi:id=""_9R-2t9PyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBActivity4"" value=""1""/>
                          <details xmi:id=""_9R-2uNPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBClassTemplateParameter"" value=""1""/>
                          <details xmi:id=""_9R-2udPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBTemplate"" value=""1""/>
                          <details xmi:id=""_9R-2utPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.net"" value=""1""/>
                          <details xmi:id=""_9R-2u9PyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.messagebroker"" value=""1""/>
                          <details xmi:id=""_9R-2vNPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBSpecificInstanceType1"" value=""1""/>
                          <details xmi:id=""_9R-2vdPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.viz.webservice.capabilty"" value=""1""/>
                          <details xmi:id=""_9R-2vtPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBSpecificInstanceType2"" value=""1""/>
                          <details xmi:id=""_9R-2v9PyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBObjectDiagram"" value=""1""/>
                          <details xmi:id=""_9R-2wNPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.os"" value=""1""/>
                          <details xmi:id=""_9R-2wdPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.systemp"" value=""1""/>
                          <details xmi:id=""_9R-2wtPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.derby"" value=""1""/>
                          <details xmi:id=""_9R-2w9PyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.os"" value=""1""/>
                          <details xmi:id=""_9R-2xNPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBStereotypedArtifact"" value=""1""/>
                          <details xmi:id=""_9R-2xdPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBTypes1"" value=""1""/>
                          <details xmi:id=""_9R-2xtPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.systemz"" value=""1""/>
                          <details xmi:id=""_9R-2x9PyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBTypes3"" value=""1""/>
                          <details xmi:id=""_9R-2yNPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.virtualization"" value=""1""/>
                          <details xmi:id=""_9R-2ydPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.ihs"" value=""1""/>
                          <details xmi:id=""_9R-2ytPyEeSa1bJT-ij9YA"" key=""com.ibm.ccl.soa.deploy.core.ui.activity.core"" value=""1""/>
                          <details xmi:id=""_9R-2y9PyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBPackageTemplateParameter"" value=""1""/>
                          <details xmi:id=""_9R-2zNPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.virtualization"" value=""1""/>
                          <details xmi:id=""_9R-2zdPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBComment3"" value=""1""/>
                          <details xmi:id=""_9R-2ztPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.modeling.enterprise.services.uireduction.activity"" value=""1""/>
                          <details xmi:id=""_9R-2z9PyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.iis"" value=""1""/>
                          <details xmi:id=""_9R-20NPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBFunctionTemplateParameter"" value=""1""/>
                          <details xmi:id=""_9R-20dPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.was"" value=""1""/>
                          <details xmi:id=""_9R-20tPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.rest.ui.uireduction.activity"" value=""1""/>
                          <details xmi:id=""_9R-209PyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.operation"" value=""1""/>
                          <details xmi:id=""_9R-21NPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.portlet"" value=""1""/>
                          <details xmi:id=""_9R-21dPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBInteractionOverview"" value=""1""/>
                          <details xmi:id=""_9R-21tPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.db2"" value=""1""/>
                          <details xmi:id=""_9R-219PyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.messaging"" value=""1""/>
                          <details xmi:id=""_9R-22NPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.storage"" value=""1""/>
                          <details xmi:id=""_9R-22dPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.server"" value=""1""/>
                          <details xmi:id=""_9R-22tPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBInteractionOverviewDiagram"" value=""1""/>
                          <details xmi:id=""_9R-229PyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.tomcat"" value=""1""/>
                          <details xmi:id=""_9R-23NPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBRelationship3"" value=""1""/>
                          <details xmi:id=""_9R-23dPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.server"" value=""1""/>
                          <details xmi:id=""_9R-23tPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.tomcat"" value=""1""/>
                          <details xmi:id=""_9R-239PyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.javaVisualizerActivity"" value=""1""/>
                          <details xmi:id=""_9R-24NPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBStereotypedDeployment1"" value=""1""/>
                          <details xmi:id=""_9R-24dPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.transform.uml2.xsd.profile.uireduction.activity"" value=""1""/>
                          <details xmi:id=""_9R-24tPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.modeling.soa.ml.uireduction.activity"" value=""1""/>
                          <details xmi:id=""_9R-249PyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.jms"" value=""1""/>
                          <details xmi:id=""_9R-25NPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.sqlserver"" value=""1""/>
                          <details xmi:id=""_9R-25dPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.http.activity.id"" value=""1""/>
                          <details xmi:id=""_9R-25tPyEeSa1bJT-ij9YA"" key=""com.ibm.ccl.soa.deploy.core.ui.activity.generic"" value=""1""/>
                          <details xmi:id=""_9R-259PyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.systemp"" value=""1""/>
                          <details xmi:id=""_9R-26NPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBCollaborationUse"" value=""1""/>
                          <details xmi:id=""_9R-26dPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.derby"" value=""1""/>
                          <details xmi:id=""_9R-26tPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBTiming"" value=""1""/>
                          <details xmi:id=""_9R-269PyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.net"" value=""1""/>
                          <details xmi:id=""_9R-27NPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.systemz"" value=""1""/>
                          <details xmi:id=""_9R-27dPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.messagebroker"" value=""1""/>
                          <details xmi:id=""_9R-27tPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.portal"" value=""1""/>
                          <details xmi:id=""_9R-279PyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBComponentTemplateParameter"" value=""1""/>
                          <details xmi:id=""_9R-28NPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.ihs"" value=""1""/>
                          <details xmi:id=""_9R-28dPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBTimingDiagram"" value=""1""/>
                          <details xmi:id=""_9R-28tPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.portal"" value=""1""/>
                          <details xmi:id=""_9R-289PyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.oracle"" value=""1""/>
                          <details xmi:id=""_9R-29NPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBStereotypedClass"" value=""1""/>
                          <details xmi:id=""_9R-29dPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBProfileApplication"" value=""1""/>
                          <details xmi:id=""_9R-29tPyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.umlBBStereotypedComponent"" value=""1""/>
                          <details xmi:id=""_9R-299PyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.db2z"" value=""1""/>
                          <details xmi:id=""_9R-2-NPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.database"" value=""1""/>
                          <details xmi:id=""_9R-2-dPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.infrastructure.http"" value=""1""/>
                          <details xmi:id=""_9R-2-tPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.oracle"" value=""1""/>
                          <details xmi:id=""_9R-2-9PyEeSa1bJT-ij9YA"" key=""com.ibm.xtools.activities.analysisAndDesign.zephyrAnalysis"" value=""1""/>
                          <details xmi:id=""_9R-2_NPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.iis"" value=""1""/>
                          <details xmi:id=""_9R-2_dPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.db2z"" value=""1""/>
                          <details xmi:id=""_9R-2_tPyEeSa1bJT-ij9YA"" key=""com.ibm.rational.deployment.activity.physical.http"" value=""1""/>
                        </eAnnotations>
                        <eAnnotations xmi:id=""_9R-2_9PyEeSa1bJT-ij9YA"" source=""com.ibm.xtools.updm.migration.marker"">
                          <details xmi:id=""_9R-3ANPyEeSa1bJT-ij9YA"" key=""SourceVersion"" value=""UPIA v1.2""/>
                          <details xmi:id=""_9R-3AdPyEeSa1bJT-ij9YA"" key=""TargetVersion"" value=""UPIA v1.3""/>
                          <details xmi:id=""_9R-3AtPyEeSa1bJT-ij9YA"" key=""UserNotified"" value=""1""/>
                        </eAnnotations>
                        <eAnnotations xmi:id=""_9R-3A9PyEeSa1bJT-ij9YA"" source=""com.ibm.xtools.upia.soaml.integration"">
                          <details xmi:id=""_9R-3BNPyEeSa1bJT-ij9YA"" key=""state"" value=""SoaMLApplied""/>
                        </eAnnotations>
                        <packageImport xmi:id=""_9R-3BdPyEeSa1bJT-ij9YA"">
                          <importedPackage xmi:type=""uml:Model"" href=""pathmap://UML_LIBRARIES/UMLPrimitiveTypes.library.uml#_0""/>
                        </packageImport>
                        <packageImport xmi:id=""_9R-3BtPyEeSa1bJT-ij9YA"">
                          <importedPackage xmi:type=""uml:Model"" href=""pathmap://UPIA_HOME/UPIAModelLibrary.emx#_1ayqoFrNEduq4eXjMcjG2g?UPIAModelLibrary?""/>
                        </packageImport>
                        <packagedElement xmi:type=""uml:Package"" xmi:id=""_9R-3B9PyEeSa1bJT-ij9YA"" name=""UPIA Model Architecture Description"">
                          <eAnnotations xmi:id=""_9R-3CNPyEeSa1bJT-ij9YA"" source=""uml2.diagrams""/>
                        </packagedElement>
                        <packagedElement xmi:type=""uml:Package"" xmi:id=""_GJaJsNPzEeSa1bJT-ij9YA"" name=""PES Data"">
                        <eAnnotations xmi:id=""_GSyIINPzEeSa1bJT-ij9YA"" source=""uml2.diagrams"" references=""_GSyIIdPzEeSa1bJT-ij9YA"">");

                    foreach (View view in views)
                    {
                        List<Thing> thing_list = new List<Thing>(view.mandatory);
                        thing_list.AddRange(view.optional);

                        writer.WriteRaw("<contents xmi:type=\"umlnotation:UMLDiagram\" xmi:id=\"" + view.id + "\" type=\"Freeform\" name=\"" + view.name + "\">");

                        if (OV1_pic_views.TryGetValue(view.id, out value))
                        {
                            thing_GUID = "_" + Guid.NewGuid().ToString("N").Substring(10);

                            thing_GUID_1 = "_02";

                            thing_GUID_3 = thing_GUID.Substring(7, 16);

                            writer.WriteRaw("<children xmi:id=\"" + thing_GUID_1 + "1111" + thing_GUID_3 + "\" type=\"skpicture\">");
                            writer.WriteRaw("<children xmi:id=\"" + thing_GUID_1 + "2222" + thing_GUID_3 + "\" type=\"skshapes\">");
                            writer.WriteRaw("<styles xmi:type=\"notation:DrawerStyle\" xmi:id=\"" + thing_GUID_1 + "3333" + thing_GUID_3 + "\"/>");
                            writer.WriteRaw("<styles xmi:type=\"notation:TitleStyle\" xmi:id=\"" + thing_GUID_1 + "4444" + thing_GUID_3 + "\"/>");
                            writer.WriteRaw("<styles xmi:type=\"notation:DrawerStyle\" xmi:id=\"" + thing_GUID_1 + "5555" + thing_GUID_3 + "\"/>");
                            writer.WriteRaw("<styles xmi:type=\"notation:TitleStyle\" xmi:id=\"" + thing_GUID_1 + "6666" + thing_GUID_3 + "\"/>");
                            writer.WriteRaw("<element xsi:nil=\"true\"/>");
                            writer.WriteRaw("</children>");
                            writer.WriteRaw("<children xmi:id=\"" + thing_GUID_1 + "7777" + thing_GUID_3 + "\" type=\"skdescription\">");
                            writer.WriteRaw("<element xsi:nil=\"true\"/>");
                            writer.WriteRaw("</children>");
                            writer.WriteRaw("<styles xmi:type=\"notation:ShapeStyle\" xmi:id=\"" + thing_GUID_1 + "8888" + thing_GUID_3 + "\" description=\"picture\" transparency=\"0\" lineWidth=\"3\" roundedBendpointsRadius=\"12\"/>");
                            writer.WriteRaw("<styles xmi:type=\"notation:LineTypeStyle\" xmi:id=\"" + thing_GUID_1 + "9999" + thing_GUID_3 + "\"/>");
                            writer.WriteRaw("<styles xmi:type=\"SketchNotation:SketchStyle\" xmi:id=\"" + thing_GUID_1 + "aaaa" + thing_GUID_3 + "\" figureOverride=\"1\" figureImageURI=\"" + value.name + "\"/>");
                            writer.WriteRaw("<styles xmi:type=\"notation:RoundedCornersStyle\" xmi:id=\"" + thing_GUID_1 + "bbbb" + thing_GUID_3 + "\"/>");
                            writer.WriteRaw("<styles xmi:type=\"notation:TextStyle\" xmi:id=\"" + thing_GUID_1 + "cccc" + thing_GUID_3 + "\" textAlignment=\"Center\"/>");
                            writer.WriteRaw("<element xsi:nil=\"true\"/>");
                            writer.WriteRaw("<layoutConstraint xmi:type=\"notation:Bounds\" xmi:id=\"" + thing_GUID_1 + "dddd" + thing_GUID_3 + "\" x=\"5706\" y=\"3804\"/>");
                            writer.WriteRaw("</children>");
                        }

                        foreach (Thing thing in thing_list)
                        {

                            thing_GUID_1 = "_00";

                            thing_GUID_3 = thing_GUIDs[thing.id];

                            if (location_dic.TryGetValue(thing.id, out location) == true)
                            {
                                loc_x = location.top_left_x;
                                loc_y = location.top_left_y;
                                size_x = (Convert.ToInt32(location.bottom_right_x) - Convert.ToInt32(location.top_left_x)).ToString();
                                size_y = (Convert.ToInt32(location.top_left_y) - Convert.ToInt32(location.bottom_right_y)).ToString();
                            }
                            else
                            {
                                loc_x = "$none$";
                                loc_y = "$none$";
                                size_x = "$none$";
                                size_y = "$none$";
                            }

                            writer.WriteRaw("<children xmi:type=\"umlnotation:UMLShape\" xmi:id=\"" + thing_GUID_1 + "1111" + thing_GUID_3 + "\" element=\"" + "_AAZZZZ" + thing_GUID_3 + "\" fontHeight=\"8\" transparency=\"0\" lineColor=\"14263149\" lineWidth=\"1\" showStereotype=\"Label\">");
                            writer.WriteRaw("<children xmi:type=\"notation:DecorationNode\" xmi:id=\"" + thing_GUID_1 + "2222" + thing_GUID_3 + "\" type=\"ImageCompartment\">");
                            writer.WriteRaw("<layoutConstraint xmi:type=\"notation:Size\" xmi:id=\"" + thing_GUID_1 + "3333" + thing_GUID_3 + "\" width=\"1320\" height=\"1320\"/></children>");
                            writer.WriteRaw("<children xmi:type=\"notation:BasicDecorationNode\" xmi:id=\"" + thing_GUID_1 + "4444" + thing_GUID_3 + "\" type=\"Stereotype\"/>");
                            writer.WriteRaw("<children xmi:type=\"notation:BasicDecorationNode\" xmi:id=\"" + thing_GUID_1 + "5555" + thing_GUID_3 + "\" type=\"Name\"/>");
                            writer.WriteRaw("<children xmi:type=\"notation:BasicDecorationNode\" xmi:id=\"" + thing_GUID_1 + "6666" + thing_GUID_3 + "\" type=\"Parent\"/>");
                            writer.WriteRaw("<children xmi:type=\"notation:SemanticListCompartment\" xmi:id=\"" + thing_GUID_1 + "7777" + thing_GUID_3 + "\" type=\"AttributeCompartment\"/>");
                            writer.WriteRaw("<children xmi:type=\"notation:SemanticListCompartment\" xmi:id=\"" + thing_GUID_1 + "8888" + thing_GUID_3 + "\" type=\"OperationCompartment\"/>");
                            writer.WriteRaw("<children xmi:type=\"notation:SemanticListCompartment\" xmi:id=\"" + thing_GUID_1 + "9999" + thing_GUID_3 + "\" visible=\"false\" type=\"SignalCompartment\"/>");
                            writer.WriteRaw("<children xmi:type=\"umlnotation:UMLShapeCompartment\" xmi:id=\"" + thing_GUID_1 + "aaaa" + thing_GUID_3 + "\" visible=\"false\" type=\"StructureCompartment\"/>");
                            writer.WriteRaw("<layoutConstraint xmi:type=\"notation:Bounds\" xmi:id=\"" + thing_GUID_1 + "bbbb" + thing_GUID_3 + "\""
                            + ((loc_x == "$none$") ? "" : " x=\"" + loc_x + "\"")
                            + ((loc_y == "$none$") ? "" : " y=\"" + loc_y + "\"")
                            + ((size_x == "$none$") ? "" : " width=\"" + size_x + "\"")
                            + ((size_y == "$none$") ? "" : " height=\"" + size_y + "\"")
                            + "/></children>");
                        }

                        writer.WriteRaw(@"<element xsi:nil=""true""/>");

                        foreach (Thing thing in thing_list)
                        {

                            thing_GUID_1 = "_00";

                            sorted_results = Get_Tuples_place1(thing, tuples);

                            foreach (List<Thing> values in sorted_results)
                            {
                                thing_GUID_2 = thing_GUIDs[values[0].place1];
                                thing_GUID_3 = thing_GUIDs[values[0].place2];

                                writer.WriteRaw("<edges xmi:type=\"umlnotation:UMLConnector\" xmi:id=\"" + thing_GUID_1 + "cccc" + thing_GUID_2 + "\" element=\"" + thing_GUID_1 + "dddd" + thing_GUID_2 + "\" source=\"" + thing_GUID_1 + "1111" + thing_GUID_2 + "\" target=\"" + thing_GUID_1 + "1111" + thing_GUID_3 + "\" fontHeight=\"8\" roundedBendpointsRadius=\"4\" routing=\"Rectilinear\" lineColor=\"8421504\" lineWidth=\"1\" showStereotype=\"Text\">");
                                writer.WriteRaw("<children xmi:type=\"notation:DecorationNode\" xmi:id=\"" + thing_GUID_1 + "eeee" + thing_GUID_2 + "\" type=\"NameLabel\">");
                                writer.WriteRaw("<children xmi:type=\"notation:BasicDecorationNode\" xmi:id=\"" + thing_GUID_1 + "ffff" + thing_GUID_2 + "\" type=\"Stereotype\"/>");
                                writer.WriteRaw("<children xmi:type=\"notation:BasicDecorationNode\" xmi:id=\"" + thing_GUID_1 + "gggg" + thing_GUID_2 + "\" type=\"Name\"/>");
                                writer.WriteRaw("<layoutConstraint xmi:type=\"notation:Bounds\" xmi:id=\"" + thing_GUID_1 + "hhhh" + thing_GUID_2 + "\" y=\"-186\"/>");
                                writer.WriteRaw("</children>");
                                writer.WriteRaw("<bendpoints xmi:type=\"notation:RelativeBendpoints\" xmi:id=\"" + thing_GUID_1 + "iiii" + thing_GUID_2 + "\" points=\"[6, 42, 32, -157]$[29, 168, 55, -31]\"/>");
                                writer.WriteRaw("</edges>");
                            }
                        }

                        writer.WriteRaw(@"</contents>
                                    </eAnnotations>");
                    }

                    foreach (KeyValuePair<string, Thing> thing in things)
                    {

                        //if (thing_GUIDs.TryGetValue(thing.Value.id, out thing_GUID) == false)
                        //{

                        //    thing_GUID = "_" + Guid.NewGuid().ToString("N").Substring(10);
                        //    thing_GUID_1 = thing_GUID.Substring(0, 3);
                        //    thing_GUID_3 = thing_GUID.Substring(7, 16);

                        //    thing_GUIDs.Add(thing.Value.id, thing_GUID_3);

                        //}

                         sorted_results = Get_Tuples_place1(thing.Value, tuples);
                         count = sorted_results.Count();
                         sorted_results.AddRange(Get_Tuples_place1(thing.Value, tuple_types));
                         count2 = sorted_results.Count();

                         if (count != 0)
                             foreach (List<Thing> values in sorted_results)
                             {
                                 thing_GUID_2 = thing_GUIDs[values[0].place1];
                                 thing_GUID_3 = thing_GUIDs[values[0].place2];

                                 writer.WriteRaw("<packagedElement xmi:type=\"uml:Class\" xmi:id=\"" + "_AAZZZZ" + thing_GUID_2 + "\" name=\"" + thing.Value.name + "\">");
                                 writer.WriteRaw("<generalization xmi:id=\"_VF0JMPAvEeSRVK9XlySZNA\" general=\"_I86dsPAvEeSRVK9XlySZNA\"/>");
                                 writer.WriteRaw("</packagedElement>");
                             }
                         else if (count2 == 1 + count)
                             foreach (List<Thing> values in sorted_results)
                             {
                                 thing_GUID_2 = thing_GUIDs[values[0].place1];
                                 thing_GUID_3 = thing_GUIDs[values[0].place2];

                                 writer.WriteRaw("<packagedElement xmi:type=\"uml:Class\" xmi:id=\"" + "_AAZZZZ" + thing_GUID_2 + "\" name=\"" + thing.Value.name + "\">");
                                 writer.WriteRaw("<ownedAttribute xmi:id=\"_cui1IPAvEeSRVK9XlySZNA\" name=\"activity 2\" visibility=\"private\" type=\"_I86dsPAvEeSRVK9XlySZNA\" aggregation=\"composite\" association=\"_cuZEIPAvEeSRVK9XlySZNA\">");
                                 writer.WriteRaw("<upperValue xmi:type=\"uml:LiteralUnlimitedNatural\" xmi:id=\"_cui1IvAvEeSRVK9XlySZNA\" value=\"*\"/>");
                                 writer.WriteRaw("<lowerValue xmi:type=\"uml:LiteralInteger\" xmi:id=\"_cui1IfAvEeSRVK9XlySZNA\"/>");
                                 writer.WriteRaw("</ownedAttribute>");
                                 writer.WriteRaw("</packagedElement>");
                             }
                         else
                         {
                             thing_GUID = thing_GUIDs[thing.Value.id];

                             writer.WriteRaw("<packagedElement xmi:type=\"uml:Class\" xmi:id=\"" + "_AAZZZZ" + thing_GUID + "\" name=\"" + thing.Value.name + "\"/>");
                         }
                    }

                    writer.WriteRaw(@"</packagedElement><profileApplication xmi:id=""_9R-3CdPyEeSa1bJT-ij9YA"">
                          <eAnnotations xmi:id=""_9R-3CtPyEeSa1bJT-ij9YA"" source=""http://www.eclipse.org/uml2/2.0.0/UML"">
                            <references xmi:type=""ecore:EPackage"" href=""pathmap://UML_PROFILES/Standard.profile.uml#_yzU58YinEdqtvbnfB2L_5w""/>
                          </eAnnotations>
                          <appliedProfile href=""pathmap://UML_PROFILES/Standard.profile.uml#_0""/>
                        </profileApplication>
                        <profileApplication xmi:id=""_9R-3C9PyEeSa1bJT-ij9YA"">
                          <eAnnotations xmi:id=""_9R-3DNPyEeSa1bJT-ij9YA"" source=""http://www.eclipse.org/uml2/2.0.0/UML"">
                            <references xmi:type=""ecore:EPackage"" href=""pathmap://UML2_MSL_PROFILES/Default.epx#_fNwoAAqoEd6-N_NOT9vsCA?Default/Default?""/>
                          </eAnnotations>
                          <appliedProfile href=""pathmap://UML2_MSL_PROFILES/Default.epx#_a_S3wNWLEdiy4IqP8whjFA?Default?""/>
                        </profileApplication>
                        <profileApplication xmi:id=""_9R-3DdPyEeSa1bJT-ij9YA"">
                          <eAnnotations xmi:id=""_9R-3DtPyEeSa1bJT-ij9YA"" source=""http://www.eclipse.org/uml2/2.0.0/UML"">
                            <references xmi:type=""ecore:EPackage"" href=""pathmap://UML2_MSL_PROFILES/Deployment.epx#_IrdAUMmBEdqBcN1R6EvWUw?Deployment/Deployment?""/>
                          </eAnnotations>
                          <appliedProfile href=""pathmap://UML2_MSL_PROFILES/Deployment.epx#_vjbuwOvHEdiDX5bji0iVSA?Deployment?""/>
                        </profileApplication>
                        <profileApplication xmi:id=""_9R-3D9PyEeSa1bJT-ij9YA"">
                          <eAnnotations xmi:id=""_9R-3ENPyEeSa1bJT-ij9YA"" source=""http://www.eclipse.org/uml2/2.0.0/UML"">
                            <references xmi:type=""ecore:EPackage"" href=""pathmap://UPIA_HOME/UPIA.epx#_7im0MEc6Ed-f1uPQXF_0HA?UPIA/UPIA?""/>
                          </eAnnotations>
                          <appliedProfile href=""pathmap://UPIA_HOME/UPIA.epx#_c2-k4GUFEduIxJjDZy3KpA?UPIA?""/>
                        </profileApplication>
                        <profileApplication xmi:id=""_9R-3EdPyEeSa1bJT-ij9YA"">
                          <eAnnotations xmi:id=""_9R-3EtPyEeSa1bJT-ij9YA"" source=""http://www.eclipse.org/uml2/2.0.0/UML"">
                            <references xmi:type=""ecore:EPackage"" href=""pathmap://SOAML/SoaML.epx#_LLGeYPc5EeGmUaPBBKwKBw?SoaML/SoaML?""/>
                          </eAnnotations>
                          <appliedProfile href=""pathmap://SOAML/SoaML.epx#_ut1IIGfDEdy6JoIZoRRqYw?SoaML?""/>
                        </profileApplication>
                      </uml:Model>
                      <UPIA:EnterpriseModel xmi:id=""_9R-3E9PyEeSa1bJT-ij9YA"" base_Package=""_9R-2X9PyEeSa1bJT-ij9YA""/>
                      <UPIA:ArchitectureDescription xmi:id=""_9R-3FNPyEeSa1bJT-ij9YA"" base_Package=""_9R-3B9PyEeSa1bJT-ij9YA""/>
                      <UPIA:View xmi:id=""_GSChQNPzEeSa1bJT-ij9YA"" base_Package=""_GJaJsNPzEeSa1bJT-ij9YA""/>");

                    foreach (KeyValuePair<string, Thing> thing in things)
                    {

                        thing_GUID = thing_GUIDs[thing.Value.id];

                        writer.WriteRaw("<UPIA:System xmi:id=\"" + "_BBZZZZ" + thing_GUID + "\" base_Class=\"" + "_AAZZZZ" + thing_GUID + "\"/>");
                    }

                    writer.WriteRaw(@"</xmi:XMI>");

                    writer.Flush();
                }
                return sw.ToString();
            }
        }
    }

}
