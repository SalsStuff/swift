using System;
using System.Collections.Generic;
using Database;

namespace Messages
{
    //https://www2.swift.com/knowledgecentre/publications/us1m_20200724/2.0?topic=mt112.htm
    public class MT112 : MTMessage
    {
        DBUtils dbu = new DBUtils();

        #region SEQUENCE_VARIABLES
        // Sequence A - Mandatory
        // Sequence A General Information contains information about the stop payment of a cheque.
        List<TagData<string, string, string, string, int>> sequenceA = new List<TagData<string, string, string, string, int>>
        {
            // Tag, Name, Value, Mandatory
            new TagData<string, string, string, string, int>("20",  "Transaction Reference Number",     "" ,"M", 0),
            new TagData<string, string, string, string, int>("21",  "Cheque Number",                   "" ,"M", 0),
            new TagData<string, string, string, string, int>("30",  "Date of Issue",                    "" ,"M", 0),
            new TagData<string, string, string, string, int>("32A", "Amount",                           "", "M", 0),
            new TagData<string, string, string, string, int>("32B", "Amount",                           "", "M", 0),
            new TagData<string, string, string, string, int>("52A", "Drawer Bank",                      "", "O", 0),
            new TagData<string, string, string, string, int>("52B", "Drawer Bank",                      "", "O", 0),
            new TagData<string, string, string, string, int>("52D", "Drawer Bank",                      "", "O", 0),
            new TagData<string, string, string, string, int>("59",  "Payee",                            "", "O", 0),
            new TagData<string, string, string, string, int>("76",  "Answers",                          "", "M", 0)
        };
        #endregion

        #region MESSAGE SETUP
        /// <summary>
        /// Method Constructor
        /// </summary>
        public MT112()
        {
            InitializeMT112();
        }

        /// <summary>
        /// Method Constructor
        /// </summary>
        /// <param name="msg"></param>
        public MT112(String msg)
        {
            InitializeMT112();

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

        private void InitializeMT112()
        {
            numOfSequences = 1;
            ResetVariables();
            DefineScope();
            Anomalies.Clear();
        }

        /// <summary>
        /// Definition of Message Scope
        /// This is the SWIFT definition of the message
        /// It can be found at https://www2.swift.com/knowledgecentre/publications/us1m_20200724/2.0?topic=mt112-scope.htm
        /// </summary>
        protected override void DefineScope()
        {
            Scope = "MT 112 Scope:\r\n" +
                    "This message type is sent by the drawee bank (on which a cheque is drawn) to the drawer bank or the bank acting on behalf of the drawer bank.\r\n" +
                    "It is used to indicate what actions have been taken in attempting to stop payment of the cheque referred to in the message.\r\n";
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
                        case "20":
                            if (Is_T20_Valid(field) == false)
                                validTag = false;
                            break;
                        case "21":
                            if (Is_T21_Valid(field) == false)
                                validTag = false;
                            break;
                        case "30":
                            if (Is_T30_Valid(field) == false)
                                validTag = false;
                            break;
                        case "32A":
                            if (Is_T32A_Valid(field) == false)
                                validTag = false;
                            break;
                        case "32B":
                            if (Is_T32B_Valid(field) == false)
                                validTag = false;
                            break;
                        case "52A":
                        case "52B":
                        case "52D":
                            if (Is_T52_Valid(field) == false)
                                validTag = false;
                            break;
                        case "59":
                            if (Is_T59_Valid(field) == false)
                                validTag = false;
                            break;
                        case "76":
                            if (Is_T76_Valid(field) == false)
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
            int T32present = 0;
            int T52present = 0;

            foreach (string sid in seqs)
            {
                seq = getSequence(sid);

                if (IsNewSequencePresent(seq) == true)
                {
                    foreach (TagData<string, string, string, string, int> t in seq)
                    {
                        validateField = true;
                        /* check for vairants */
                        if (sid.Equals("A") == true)
                        {
                            if ((t.Tag.Equals("32A") && t.Present == 0) || (t.Tag.Equals("32B") && t.Present == 0))
                            {
                                validateField = false;
                                T32present++;
                            }
                            if ((t.Tag.Equals("52A") && t.Present == 0) || (t.Tag.Equals("52B") && t.Present == 0) || (t.Tag.Equals("52D") && t.Present == 0))
                            {
                                validateField = false;
                                T52present++;
                            }
                        }

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

                if ((sid.Equals("A") == true) && (T32present == 3))
                    Anomalies.Add("ERROR: Mandatory Tag 32 is not present in any variantion");
                if ((sid.Equals("A") == true) && (T52present == 3))
                    Anomalies.Add("ERROR: Mandatory Tag 52 is not present in any variantion");
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
        /// Returns the number of the cheque to which this message refers.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT21_ChequeNumber(List<TagData<string, string, string, string, int>> seq)
        {
            return getT21(seq);
        }

        /// <summary>
        /// getT30_DateOfIssue
        /// 
        /// Returns the date on which the cheque was drawn.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT30_DateOfIssue(List<TagData<string, string, string, string, int>> seq)
        {
            return getT30(seq);
        }

        /// <summary>
        /// getT32A_Date
        /// 
        /// Returns the date on which the cheque was drawn.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT32A_Date(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT32A(seq);

            return data[0];
        }

        /// <summary>
        /// getT32A_Currency
        /// 
        /// Returns the currency on which the cheque was drawn.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT32A_Currency(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT32A(seq);

            return data[1];
        }

        /// <summary>
        /// getT32A_Amount
        /// 
        /// Returns the amount of the cheque drawn.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public Nullable<double> getT32A_Amount(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT32A(seq);
            Nullable<double> amount = null;

            if (data[2] != null)
                amount = Convert.ToDouble(data[2]);

            return amount;
        }

        /// <summary>
        /// getT32B_Currency
        /// 
        /// Returns the currency on which the cheque was drawn.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT32B_Currency(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT32B(seq);

            return data[0];
        }

        /// <summary>
        /// getT32B_Amount
        /// 
        /// Returns the amount of the cheque drawn.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public Nullable<double> getT32B_Amount(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> data = getT32B(seq);
            Nullable<double> amount = null;

            if (data[1] != null)
                amount = Convert.ToDouble(data[1]);

            return amount;
        }

        /// <summary>
        /// getT52A_ID
        /// 
        /// Returns the id of the financial institution from which party A will transfer the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT52A_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT52A(seq);

            return lst[0];
        }

        /// <summary>
        /// getT52A_Code
        /// 
        /// Returns the code of the financial institution from which party A will transfer the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT52A_Code(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT52A(seq);

            return lst[1];
        }

        /// <summary>
        /// getT52B_ID
        /// 
        /// Returns the id of the financial institution from which party A will transfer the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT52B_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT52B(seq);

            return lst[0];
        }

        /// <summary>
        /// getT52B_Location
        /// 
        /// Returns the location of the financial institution from which party A will transfer the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT52B_Location(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT52B(seq);

            return lst[1];
        }

        /// <summary>
        /// getT52D_ID
        /// 
        /// Returns the id of the financial institution from which party A will transfer the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT52D_ID(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT52D(seq);

            return lst[0];
        }

        /// <summary>
        /// getT52D_NameAddr
        /// 
        /// Returns the name and address of the financial institution from which party A will transfer the funds.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT52D_NameAddr(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> lst = getT52D(seq);

            return lst[1];
        }

        /// <summary>
        /// getT59_PayeeAccount
        /// 
        /// Returns the identifies the beneficiary of the cheque.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT59_PayeeAccount(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> retLst = getT59(seq);

            return retLst[0];
        }

        /// <summary>
        /// getT59_PayeeNameAddr
        /// 
        /// Returns the identifies the beneficiary of the cheque.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT59_PayeeNameAddr(List<TagData<string, string, string, string, int>> seq)
        {
            List<string> retLst = getT59(seq);

            return retLst[1];
        }

        /// <summary>
        /// getT76_Answers
        /// 
        /// Returns information as to whether or not the stop payment has been effected. 
        /// In addition, a response should be given to any request for reimbursement authorisation.
        /// </summary>
        /// <param name="seq"></param>
        /// <returns></returns>
        public string getT76_Answers(List<TagData<string, string, string, string, int>> seq)
        {
            return getT76(seq);
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
                    sqlCmd = "Select max(reference_id) from dbo.MT112_Block1";
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
                sqlCmd = "INSERT INTO dbo.MT112_Block1 (reference_id, application_id, service_id, lt_address, bic_code, logical_terminal_code, bic_branch_code, session_number, sequence_number) ";
                sqlCmd += "VALUES('" + refid + "', '" + hdr.ApplicationID + "', '" + hdr.ServiceID + "', '" + hdr.LTAddress + "', '" + hdr.BICCode + "', '" + hdr.LogicalTerminalCode + "', '" + hdr.BICBranchCode + "', '" + hdr.SessionNumber + "', '" + hdr.SequenceNumber + "')";

                dbu.saveMTRecord(sqlCmd);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to insert MT112 Block1 record.\n" + ex.Message);
            }
        }

        private void saveBlock2(long refid, BlockHeader hdr)
        {
            string sqlCmd = null;

            if (hdr == null)
                return;

            try
            {
                sqlCmd = "INSERT INTO dbo.MT112_Block2 (reference_id, input_output_id, message_type, destination_address, priority, delivery_monitoring, ";
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
                throw new Exception("Failed to insert MT112 Block2 record.\n" + ex.Message);
            }
        }

        private void saveBlock3(long refid, BlockHeader hdr)
        {
            string sqlCmd = null;

            if (hdr == null)
                return;

            try
            {
                sqlCmd = "INSERT INTO dbo.MT112_Block3 (reference_id, tag103_service_id, tag113_banking_priority, tag108_mur, tag119_validation_flag, ";
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
                throw new Exception("Failed to insert MT112 Block3 record.\n" + ex.Message);
            }
        }

        private void saveBlock4(long refid)
        {
            string sqlCmd = null;

            try
            {
                sqlCmd = "INSERT INTO dbo.MT112_SequenceA (reference_id, transaction_reference_number_20, cheque_number_21, date_of_issue_30, date_32a, currency_32a, ";
                sqlCmd += "amount_32a, currency_32b, amount_32b, party_id_52a, party_code_52a, party_id_52b, party_location_52b, party_id_52d, party_name_addr_52d, ";
                sqlCmd += "payee_account_59, payee_name_addr_59, answers_76)";
                sqlCmd += "VALUES ('" + refid + "', '" +
                                    getT20_TransactionReferenceNumber(sequenceA) + "', '" +
                                    getT21_ChequeNumber(sequenceA) + "', '" +
                                    getT30_DateOfIssue(sequenceA) + "', '" +
                                    getT32A_Date(sequenceA) + "', '" +
                                    getT32A_Currency(sequenceA) + "', '" +
                                    getT32A_Amount(sequenceA) + "', '" +
                                    getT32B_Currency(sequenceA) + "', '" +
                                    getT32B_Amount(sequenceA) + "', '" +
                                    getT52A_ID(sequenceA) + "', '" +
                                    getT52A_Code(sequenceA) + "', '" +
                                    getT52B_ID(sequenceA) + "', '" +
                                    getT52B_Location(sequenceA) + "', '" +
                                    getT52D_ID(sequenceA) + "', '" +
                                    getT52D_NameAddr(sequenceA) + "', '" +
                                    getT59_PayeeAccount(sequenceA) + "', '" +
                                    getT59_PayeeNameAddr(sequenceA) + "', '" +
                                    getT76_Answers(sequenceA) + "')";
                dbu.saveMTRecord(sqlCmd);

                
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to insert MT112 Block4 record.\n" + ex.Message);
            }
        }

        private void saveBlock5(long refid, BlockHeader hdr)
        {
            string sqlCmd = null;

            if (hdr == null)
                return;

            try
            {
                sqlCmd = "INSERT INTO dbo.MT112_Block5 (reference_id, checksum, tng_message, pde, pde_time, pde_mir, pde_mir_date, pde_mir_lt_id, ";
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
                throw new Exception("Failed to insert MT112 Block5 record.\n" + ex.Message);
            }
        }
        #endregion


        public void testFunctions()
        {
            

        }
    }
}
