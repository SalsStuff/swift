using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database;

namespace Messages
{
    //https://www2.swift.com/knowledgecentre/publications/us1m_20200724/2.0?topic=mt105-format-spec.htm
    public class MT105 : MTMessage
    {
        DBUtils dbu = new DBUtils();

        #region SEQUENCE_VARIABLES
        // Sequence A - Mandatory
        // Sequence A General Information contains information about the request of a stop payment of a cheque.
        List<TagData<string, string, string, string, int>> sequenceA = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("27",  "Sequence of Total",                "" ,"M", 0),
            new TagData<string, string, string, string, int>("20",  "Transaction Reference Number",     "" ,"M", 0),
            new TagData<string, string, string, string, int>("21",  "Related Reference",                "" ,"M", 0),
            new TagData<string, string, string, string, int>("12",  "Sub-Message Type",                 "" ,"M", 0),
            new TagData<string, string, string, string, int>("77F", "EDIFACT Message",                  "", "M", 0)
        };
        #endregion

        #region MESSAGE SETUP
        /// <summary>
        /// Method Constructor
        /// </summary>
        public MT105()
        {
            InitializeMT105();
        }

        /// <summary>
        /// Method Constructor
        /// </summary>
        /// <param name="msg"></param>
        public MT105(String msg)
        {
            InitializeMT105();

            if (msg.Contains("{4:") == true)
                ParseBlock4(msg);
            if (msg.Contains("||") == true)
                ParsePipeMsg(msg);
        }

        /// <summary>
        /// Get method to return specified data sequence
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public List<TagData<string, string, string, string, int>> this[int index]
        {
            get
            {
                if (index == 0) { return sequenceA; }
                else { return null; }
            }
        }

        private void InitializeMT105()
        {
            numOfSequences = 1;
            ResetVariables();
            DefineScope();
            Anomalies.Clear();
        }

        /// <summary>
        /// Definition of Message Scope
        /// This is the SWIFT definition of the message
        /// It can be found at https://www2.swift.com/knowledgecentre/publications/us1m_20200724/2.0?topic=mt105-scope.htm
        /// </summary>
        protected override void DefineScope()
        {
            Scope = "MT 105 Scope:\r\n" +
                    "This message is sent by a financial institution to another financial institution. It is used as an envelope to convey an EDIFACT message.\r\n";
        }

        /// <summary>
        /// Reset the class variables 
        /// </summary>
        protected override void ResetVariables()
        {
            foreach (TagData<string, string, string, string, int> t in sequenceA)
            {
                t.Value = "";
                t.Present = 0;
            }
        }

        /// <summary>
        /// Parse block 4 of the SWIFT message - the message itself
        /// </summary>
        /// <param name="message"></param>
        private void ParseBlock4(string message)
        {
            string[] result = ParseBlock4MessageString(message);

            FillDataTags(result);

            IsMessageValid();
        }

        protected override List<TagData<string, string, string, string, int>> getSequence(string seqId)
        {
            List<TagData<string, string, string, string, int>> sequence = null;

            switch (seqId)
            {
                case "A":
                    sequence = sequenceA;
                    break;
                default:
                    break;
            }
            return sequence;
        }
        #endregion

        #region GET SET FUNCTIONS
        /// <summary>
        /// Get / Set method to auto validate message.
        /// If set to true the message will be validated when read in.
        /// If set to false an explicit call will be needed after the message is read in.
        /// </summary>
        public bool AutoValidate { get; set; } = true;
        #endregion

        #region VALIDATION RULES
        public bool IsMessageValid()
        {
            bool validMessage = true;

            if (AutoValidate == true)
            {
                ValidateTags();

                if (Anomalies.Count > 0)
                    validMessage = false;
            }
            return validMessage;
        }

        /// <summary>
        /// Validate an individual MT320 tag
        /// </summary>
        /// <param name="sequence"></param>
        /// <param name="tag"></param>
        /// <param name="tagvalue"></param>
        /// <returns></returns>
        private bool ValidateTag(string sequence, TagData<string, string, string, string, int> field)
        {
            bool validTag = true;

            switch (sequence)
            {
                case "A":
                    switch (field.Tag)
                    {
                        case "12":
                            if (Is_T12_Valid(field) == false)
                                validTag = false;
                            break;
                        case "20":
                            if (Is_T20_Valid(field) == false)
                                validTag = false;
                            break;
                        case "21":
                            if (Is_T21_Valid(field) == false)
                                validTag = false;
                            break;
                        case "27":
                            if (Is_T27_Valid(field) == false)
                                validTag = false;
                            break;
                        case "77F":
                            if (Is_T77F_Valid(field) == false)
                                validTag = false;
                            break;
                        default:
                            Anomalies.Add("ERROR : Unknown tag " + field.Tag + " in sequence A - can not validate.");
                            break;
                    }
                    break;
                default:
                    Anomalies.Add("ERROR : Unknown sequence " + sequence + " passed to ValidateTag.");
                    break;
            }

            return validTag;
        }

        /// <summary>
        /// Validate all tags in message
        /// </summary>
        private bool ValidateTags()
        {
            bool allTagsValid = true;
            List<string> seqs = new List<string>() { "A" };
            List<TagData<string, string, string, string, int>> seq;
            bool validateField = true;

            foreach (string sid in seqs)
            {
                seq = getSequence(sid);

                if (IsNewSequencePresent(seq) == true)
                {
                    foreach (TagData<string, string, string, string, int> t in seq)
                    {
                        validateField = true;
                         if (validateField == true)
                        {
                            if (ValidateTag(sid, t) == false)
                            {
                                Anomalies.Add("Tag " + t.Tag + " : Failed validation.");
                                allTagsValid = false;
                            }
                        }
                    }
                }
            }

            return allTagsValid;
        }

        #region FIELD VALIDATIONS
        #region SEQUENCE A
        
        #endregion
        #endregion
        #endregion

        #region TAG PARSING
        /// <summary>
        /// getT12_SubMessageType
        /// 
        /// Returns the  identification of the EDIFACT message contained within field 77F by its recognized numeric code.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT12_SubMessageType(List<TagData<string, string, string, string, int>> seq)
        {
            return getT12(seq);
        }

        /// <summary>
        /// getT20_TransactionReferenceNumber
        /// 
        /// Returns the reference assigned by the Sender to unambiguously identify the message.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT20_TransactionReferenceNumber(List<TagData<string, string, string, string, int>> seq)
        {
            return getT20(seq);
        }

        /// <summary>
        /// getT21_RelatedReference
        /// 
        /// Returns the  reference to the associated (enveloped) EDIFACT message.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT21_RelatedReference(List<TagData<string, string, string, string, int>> seq)
        {
            return getT21(seq);
        }

        /// <summary>
        /// getT27_MessageNumber
        /// 
        /// Returns the message number in series
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public int getT27_MessageNumber(List<TagData<string, string, string, string, int>> seq)
        {
            List<int> data = getT27(seq);

            return data[0];
        }

        /// <summary>
        /// getT27_SequenceNumber
        /// 
        /// Returns the sequence number for this series
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public int getT27_SequenceNumber(List<TagData<string, string, string, string, int>> seq)
        {
            List<int> data = getT27(seq);

            return data[1];
        }

        /// <summary>
        /// getT77F_EDIFACTMessage
        /// 
        /// Returns the EDIFACT message being sent by the Sender to the Receiver.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT77F_EDIFACTMessage(List<TagData<string, string, string, string, int>> seq)
        {
            return getT77F(seq);
        }
        #endregion

        #region SAVE DATA
        public void saveRecord(BlockHeader headers)
        {
            string sqlCmd = null;
            long ref_id = -1;

            if (headers != null)
            {
                try
                {
                    sqlCmd = "Select max(reference_id) from dbo.MT105_Block1";
                    ref_id = dbu.getNewReferenceId(sqlCmd, -1);

                    dbu.DBBegin(ref_id.ToString());
                    saveBlock1(ref_id, headers);
                    saveBlock2(ref_id, headers);
                    saveBlock3(ref_id, headers);
                    saveBlock4(ref_id);
                    saveBlock5(ref_id, headers);
                    dbu.DBCommit(ref_id.ToString());
                }
                catch (Exception ex)
                {
                    dbu.DBRollback(ref_id.ToString());
                    throw ex;
                }
            }
        }

        private void saveBlock1(long refid, BlockHeader hdr)
        {
            string sqlCmd = null;

            if (hdr == null)
                return;

            try
            {
                sqlCmd = "INSERT INTO dbo.MT105_Block1 (reference_id, application_id, service_id, lt_address, bic_code, logical_terminal_code, bic_branch_code, session_number, sequence_number) ";
                sqlCmd += "VALUES('" + refid + "', '" + hdr.ApplicationID + "', '" + hdr.ServiceID + "', '" + hdr.LTAddress + "', '" + hdr.BICCode + "', '" + hdr.LogicalTerminalCode + "', '" + hdr.BICBranchCode + "', '" + hdr.SessionNumber + "', '" + hdr.SequenceNumber + "')";

                dbu.saveMTRecord(sqlCmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to insert MT111 Block1 record.\n" + ex.Message);
            }
        }

        private void saveBlock2(long refid, BlockHeader hdr)
        {
            string sqlCmd = null;

            if (hdr == null)
                return;

            try
            {
                sqlCmd = "INSERT INTO dbo.MT105_Block2 (reference_id, input_output_id, message_type, destination_address, priority, delivery_monitoring, ";
                sqlCmd += "obsolescence_period, input_time, mir, mir_sender_date, mir_lt_address, mir_bic_code,  mir_lt_code, mir_bic_branch_code, ";
                sqlCmd += "mir_session_number, mir_sequence_number, output_date, output_time) ";
                sqlCmd += "VALUES('" + refid + "', '" + hdr.InputOutputID + "', '" + hdr.MessageType + "', '" + hdr.DestinationAddress + "', '";
                sqlCmd += hdr.Priority + "', '" + hdr.DeliveryMonitoring + "', '" + hdr.ObsolescencePeriod + "', '" + hdr.InputTime + "', '";
                sqlCmd += hdr.MIR + "', '" + hdr.MIRSenderDate + "', '" + hdr.MIRLTAddress + "', '" + hdr.MIRBICCode + "',  '" + hdr.MIRLTCode + "', '";
                sqlCmd += hdr.BICBranchCode + "', '" + hdr.MIRSessNum + "', '" + hdr.MIRSeqNum + "', '" + hdr.OutputDate + "', '" + hdr.OutputTime + "')";

                dbu.saveMTRecord(sqlCmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to insert MT111 Block2 record.\n" + ex.Message);
            }
        }

        private void saveBlock3(long refid, BlockHeader hdr)
        {
            string sqlCmd = null;

            if (hdr == null)
                return;

            try
            {
                sqlCmd = "INSERT INTO dbo.MT105_Block3 (reference_id, tag103_service_id, tag113_banking_priority, tag108_mur, tag119_validation_flag, ";
                sqlCmd += "tag423_balance_check_point, tag106_mir, tag424_related_reference, tag111_service_type_id, tag121_unique_tran_reference, ";
                sqlCmd += "tag115_addressee_info, tag165_payment_rir, tag433_sanctions_sir, tag434_payment_cir) ";
                sqlCmd += "VALUES('" + refid + "', '" + hdr.TAG103_ServiceID + "', '" + hdr.TAG113_BankingPriority + "', '" + hdr.TAG108_MUR + "', '";
                sqlCmd += hdr.TAG119_ValidationFlag + "', '" + hdr.TAG423_BalanceCheckPoint + "', '" + hdr.TAG106_MIR + "', '" + hdr.TAG424_RelatedReference + "', '";
                sqlCmd += hdr.TAG111_ServiceTypeID + "', '" + hdr.TAG121_UniqueTranReference + "', '" + hdr.TAG115_AddresseeInfo + "', '" + hdr.TAG165_PaymentRIR + "', '";
                sqlCmd += hdr.TAG433_SanctionsSIR + "', '" + hdr.TAG434_PaymentCIR + "')";

                dbu.saveMTRecord(sqlCmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to insert MT111 Block3 record.\n" + ex.Message);
            }
        }

        private void saveBlock4(long refid)
        {
            string sqlCmd = null;

            try
            {
                sqlCmd = "INSERT INTO dbo.MT105_SequenceA (reference_id, message_number_27, sequence_number_27, transaction_reference_number_20, related_reference_21, ";
                sqlCmd += "sub_message_type_12, edifact_message_77f)";
                sqlCmd += "VALUES ('" + refid + "', '" +
                                    getT27_MessageNumber(sequenceA) + "', '" +
                                    getT27_SequenceNumber(sequenceA) + "', '" +
                                    getT20_TransactionReferenceNumber(sequenceA) + "', '" +
                                    getT21_RelatedReference(sequenceA) + "', '" +
                                    getT12_SubMessageType(sequenceA) + "', '" +
                                    getT77F_EDIFACTMessage(sequenceA) + "')";
                dbu.saveMTRecord(sqlCmd);

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to insert MT111 Block4 record.\n" + ex.Message);
            }
        }

        private void saveBlock5(long refid, BlockHeader hdr)
        {
            string sqlCmd = null;

            if (hdr == null)
                return;

            try
            {
                sqlCmd = "INSERT INTO dbo.MT105_Block5 (reference_id, checksum, tng_message, pde, pde_time, pde_mir, pde_mir_date, pde_mir_lt_id, ";
                sqlCmd += "pde_mir_session_number, pde_mir_sequence_number, dlm, mrf, mrf_date, mrf_time, mrf_mir, pdm, pdm_time,  pdm_mor, ";
                sqlCmd += "pdm_mor_date, pdm_mor_lt_id, pdm_mor_session_number, pdm_mor_sequence_number, sys, sys_time, sys_mor, sys_mor_date, ";
                sqlCmd += "sys_mor_lt_id, sys_mor_session_number, sys_mor_sequence_number)";
                sqlCmd += "VALUES('" + refid + "', '" + hdr.Checksum + "', '" + hdr.TNGMessage + "', '" + hdr.PDE + "', '" + hdr.PDETime + "', '";
                sqlCmd += hdr.PDEMir + "', '" + hdr.PDEMirDate + "', '" + hdr.PDEMirLTId + "', '" + hdr.PDEMirSessionNum + "', '" + hdr.PDEMirSequenceNum + "', '";
                sqlCmd += hdr.DLM + "', '" + hdr.MRF + "', '" + hdr.MRFDate + "', '" + hdr.MRFTime + "', '" + hdr.MRFMir + "', '" + hdr.PDM + "', '";
                sqlCmd += hdr.PDMTime + "', '" + hdr.PDMMor + "', '" + hdr.PDMMorDate + "', '" + hdr.PDMMorLTId + "', '" + hdr.PDMMorSessionNum + "', '";
                sqlCmd += hdr.PDMMorSequenceNum + "', '" + hdr.SYS + "', '" + hdr.SYSTime + "', '" + hdr.SYSMor + "', '" + hdr.SYSMorDate + "', '";
                sqlCmd += hdr.SYSMorLTId + "', '" + hdr.SYSMorSessionNum + "', '" + hdr.SYSMorSequenceNum + "')";

                dbu.saveMTRecord(sqlCmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to insert MT111 Block5 record.\n" + ex.Message);
            }
        }
        #endregion

        public void testFunctions()
        {
            List<string> data;

            data = getT13C(sequenceA);

        }
    }
}
