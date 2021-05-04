CREATE TABLE "MT320_Block1" (
	"reference_id" BIGINT NOT NULL,
	"application_id" VARCHAR(50) NULL,
	"service_id" VARCHAR(50) NULL,
	"lt_address" VARCHAR(50) NULL,
	"bic_code" VARCHAR(50) NULL,
	"logical_terminal_code" VARCHAR(50) NULL,
	"bic_branch_code" VARCHAR(50) NULL,
	"session_number" VARCHAR(50) NULL,
	"sequence_number" VARCHAR(50) NULL,
); 
CREATE_INDEX IDX_UNIQUE_CLUSTERED MT320_Block1_idx MT320_Block1  [reference_id] 

CREATE TABLE "MT320_Block2" (
	"reference_id" BIGINT NOT NULL,
	"input_output_id" VARCHAR(50) NULL,
	"message_type" VARCHAR(50) NULL,
	"destination_address" VARCHAR(50) NULL,
	"priority" VARCHAR(50) NULL,
	"delivery_monitoring" VARCHAR(50) NULL,
	"obsolescence_period" VARCHAR(50) NULL,
	"input_time" VARCHAR(50) NULL,
	"mir" VARCHAR(50) NULL,
	"mir_sender_date" varchar(50) NULL,
	"mir_lt_address" varchar(50) NULL,
	"mir_bic_code" varchar(50) NULL,
	"mir_lt_code" varchar(50) NULL,
	"mir_bic_branch_code" varchar(50) NULL,
	"mir_session_number" varchar(50) NULL,
	"mir_sequence_number" varchar(50) NULL,
	"output_date" varchar(50) NULL,
	"output_time" varchar(50) NULL,
); 
CREATE_INDEX IDX_UNIQUE_CLUSTERED MT320_Block2_idx MT320_Block2  [reference_id] 

CREATE TABLE "MT320_Block3" (
	"reference_id" BIGINT NOT NULL,
	"tag103_service_id" VARCHAR(50) NULL,
	"tag113_banking_priority" VARCHAR(50) NULL,
	"tag108_mur" VARCHAR(50) NULL,
	"tag119_validation_flag" VARCHAR(50) NULL,
	"tag423_balance_check_point" VARCHAR(50) NULL,
	"tag106_mir" VARCHAR(50) NULL,
	"tag424_related_reference" VARCHAR(50) NULL,
	"tag111_service_type_id" VARCHAR(50) NULL,
	"tag121_unique_tran_reference" varchar(50) NULL,
	"tag115_addressee_info" varchar(50) NULL,
	"tag165_payment_rir" varchar(50) NULL,
	"tag433_sanctions_sir" varchar(50) NULL,
	"tag434_payment_cir" varchar(50) NULL,
); 
CREATE_INDEX IDX_UNIQUE_CLUSTERED MT320_Block3_idx MT320_Block3  [reference_id] 

CREATE TABLE "MT320_Block5" (
	"reference_id" BIGINT NOT NULL,
	"checksum" VARCHAR(50) NULL,
	"tng_message" VARCHAR(50) NULL,
	"pde" VARCHAR(50) NULL,
	"pde_time" VARCHAR(50) NULL,
	"pde_mir" VARCHAR(50) NULL,
	"pde_mir_date" VARCHAR(50) NULL,
	"pde_mir_lt_id" VARCHAR(50) NULL,
	"pde_mir_session_number" VARCHAR(50) NULL,
	"pde_mir_sequence_number" varchar(50) NULL,
	"dlm" varchar(50) NULL,
	"mrf" varchar(50) NULL,
	"mrf_date" varchar(50) NULL,
	"mrf_time" varchar(50) NULL,
	"mrf_mir" varchar(50) NULL,
	"pdm" varchar(50) NULL,
	"pdm_time" varchar(50) NULL,
	"pdm_mor" varchar(50) NULL,
	"pdm_mor_date" VARCHAR(50) NULL,
	"pdm_mor_lt_id" VARCHAR(50) NULL,
	"pdm_mor_session_number" varchar(50) NULL,
	"pdm_mor_sequence_number" varchar(50) NULL,
	"sys" varchar(50) NULL,
	"sys_time" varchar(50) NULL,
	"sys_mor" varchar(50) NULL,
	"sys_mor_date" varchar(50) NULL,
	"sys_mor_lt_id" varchar(50) NULL,
	"sys_mor_session_number" varchar(50) NULL,
	"sys_mor_sequence_number" varchar(50) NULL,
); 
CREATE_INDEX IDX_UNIQUE_CLUSTERED MT320_Block5_idx MT320_Block5  [reference_id] 

CREATE TABLE "MT320_SequenceA" (
	"reference_id" BIGINT NOT NULL,
	"senders_ref_20" varchar(50) NULL,
	"related_ref_21" varchar(50) NULL,
	"operation_type_22a" varchar(50) NULL,
	"operation_scope_94a" varchar(50) NULL,
	"event_type_22b" varchar(50) NULL,
	"common_ref_22c" varchar(50) NULL,
	"party_a_contact_num_21n" varchar(50) NULL,
	"party_a_id_82a" varchar(50) NULL,
	"party_a_code_82a" varchar(50) NULL,
	"party_a_id_82d" varchar(50) NULL,
	"party_a_addr_82d" varchar(150) NULL,
	"party_a_id_82j" varchar(250) NULL,
	"party_b_id_87a" varchar(50) NULL,
	"party_b_code_87a" varchar(50) NULL,
	"party_b_id_87d" varchar(50) NULL,
	"party_b_addr_87d" varchar(150) NULL,
	"party_b_id_87j" varchar(250) NULL,
	"fund_party_id_83a" varchar(50) NULL,
	"fund_party_code_83a" varchar(50) NULL,
	"fund_party_id_83d" varchar(50) NULL,
	"fund_party_addr_83d" varchar(150) NULL,
	"fund_party_id_83j" varchar(250) NULL,
	"terms_77d"	varchar(250) NULL,
);
CREATE_INDEX IDX_UNIQUE_CLUSTERED MT320_SequenceA_idx MT320_SequenceA  [reference_id] 

CREATE TABLE "MT320_SequenceB" (
	"reference_id" BIGINT NOT NULL,
	"party_a_role_17r" varchar(50) NULL,
	"trade_date_30t" date NULL,
	"value_date_30v" date NULL,
	"maturity_date_30p" date NULL,
	"principal_currency_32b" varchar(50) NULL,
	"principal_amount_32b" float NULL,
	"settle_amount_ccy_32h" varchar(50) NULL,
	"settle_amount_32h" float NULL,
	"next_interest_due_date_30x" date NULL,
	"interest_currency_34e" varchar(50) NULL,
	"interest_amount_34e" float NULL,
	"interest_rate_37g" float NULL,
	"day_count_fraction_14d" varchar(50) NULL,
	"last_day_first_interest_period_30f" date NULL,
	"number_of_days_indicator_38j" varchar(50) NULL,
	"number_of_days_38j" int NULL,
	"payment_clearing_center_39m" varchar(50) NULL,
);
CREATE_INDEX IDX_UNIQUE_CLUSTERED MT320_SequenceB_idx MT320_SequenceB  [reference_id] 

CREATE TABLE "MT320_SequenceC" (
	"reference_id" BIGINT NOT NULL,
	"delevery_agent_id_53a" varchar(50) NULL,
	"delevery_agent_code_53a" varchar(50) NULL,
	"delevery_agent_id_53d" varchar(50) NULL,
	"delevery_agent_addr_53d" varchar(150) NULL,
	"delevery_agent_id_53j" varchar(250) NULL,
	"intermediary_2_id_86a" varchar(50) NULL,
	"intermediary_2_code_86a" varchar(50) NULL,
	"intermediary_2_id_86d" varchar(50) NULL,
	"intermediary_2_addr_86d" varchar(150) NULL,
	"intermediary_2_id_86j" varchar(250) NULL,
	"intermediary_id_56a" varchar(50) NULL,
	"intermediary_code_56a" varchar(50) NULL,
	"intermediary_id_56d" varchar(50) NULL,
	"intermediary_addr_56d" varchar(150) NULL,
	"intermediary_id_56j" varchar(250) NULL,
	"receiving_agent_id_57a" varchar(50) NULL,
	"receiving_agent_code_57a" varchar(50) NULL,
	"receiving_agent_id_57d" varchar(50) NULL,
	"receiving_agent_addr_57d" varchar(150) NULL,
	"receiving_agent_id_57j" varchar(250) NULL,
	"beneficiary_inst_id_58a" varchar(50) NULL,
	"beneficiary_inst_code_58a" varchar(50) NULL,
	"beneficiary_inst_id_58d" varchar(50) NULL,
	"beneficiary_inst_addr_58d" varchar(150) NULL,
	"beneficiary_inst_id_58j" varchar(250) NULL,
);
CREATE_INDEX IDX_UNIQUE_CLUSTERED MT320_SequenceC_idx MT320_SequenceC  [reference_id] 

CREATE TABLE "MT320_SequenceD" (
	"reference_id" BIGINT NOT NULL,
	"delevery_agent_id_53a" varchar(50) NULL,
	"delevery_agent_code_53a" varchar(50) NULL,
	"delevery_agent_id_53d" varchar(50) NULL,
	"delevery_agent_addr_53d" varchar(150) NULL,
	"delevery_agent_id_53j" varchar(250) NULL,
	"intermediary_2_id_86a" varchar(50) NULL,
	"intermediary_2_code_86a" varchar(50) NULL,
	"intermediary_2_id_86d" varchar(50) NULL,
	"intermediary_2_addr_86d" varchar(150) NULL,
	"intermediary_2_id_86j" varchar(250) NULL,
	"intermediary_id_56a" varchar(50) NULL,
	"intermediary_code_56a" varchar(50) NULL,
	"intermediary_id_56d" varchar(50) NULL,
	"intermediary_addr_56d" varchar(150) NULL,
	"intermediary_id_56j" varchar(250) NULL,
	"receiving_agent_id_57a" varchar(50) NULL,
	"receiving_agent_code_57a" varchar(50) NULL,
	"receiving_agent_id_57d" varchar(50) NULL,
	"receiving_agent_addr_57d" varchar(150) NULL,
	"receiving_agent_id_57j" varchar(250) NULL,
	"beneficiary_inst_id_58a" varchar(50) NULL,
	"beneficiary_inst_code_58a" varchar(50) NULL,
	"beneficiary_inst_id_58d" varchar(50) NULL,
	"beneficiary_inst_addr_58d" varchar(150) NULL,
	"beneficiary_inst_id_58j" varchar(250) NULL,
);
CREATE_INDEX IDX_UNIQUE_CLUSTERED MT320_SequenceD_idx MT320_SequenceD  [reference_id] 

CREATE TABLE "MT320_SequenceE" (
	"reference_id" BIGINT NOT NULL,
	"delevery_agent_id_53a" varchar(50) NULL,
	"delevery_agent_code_53a" varchar(50) NULL,
	"delevery_agent_id_53d" varchar(50) NULL,
	"delevery_agent_addr_53d" varchar(150) NULL,
	"delevery_agent_id_53j" varchar(250) NULL,
	"intermediary_2_id_86a" varchar(50) NULL,
	"intermediary_2_code_86a" varchar(50) NULL,
	"intermediary_2_id_86d" varchar(50) NULL,
	"intermediary_2_addr_86d" varchar(150) NULL,
	"intermediary_2_id_86j" varchar(250) NULL,
	"intermediary_id_56a" varchar(50) NULL,
	"intermediary_code_56a" varchar(50) NULL,
	"intermediary_id_56d" varchar(50) NULL,
	"intermediary_addr_56d" varchar(150) NULL,
	"intermediary_id_56j" varchar(250) NULL,
	"receiving_agent_id_57a" varchar(50) NULL,
	"receiving_agent_code_57a" varchar(50) NULL,
	"receiving_agent_id_57d" varchar(50) NULL,
	"receiving_agent_addr_57d" varchar(150) NULL,
	"receiving_agent_id_57j" varchar(250) NULL,
	"beneficiary_inst_id_58a" varchar(50) NULL,
	"beneficiary_inst_code_58a" varchar(50) NULL,
	"beneficiary_inst_id_58d" varchar(50) NULL,
	"beneficiary_inst_addr_58d" varchar(150) NULL,
	"beneficiary_inst_id_58j" varchar(250) NULL,
);
CREATE_INDEX IDX_UNIQUE_CLUSTERED MT320_SequenceE_idx MT320_SequenceE  [reference_id] 

CREATE TABLE "MT320_SequenceF" (
	"reference_id" BIGINT NOT NULL,
	"delevery_agent_id_53a" varchar(50) NULL,
	"delevery_agent_code_53a" varchar(50) NULL,
	"delevery_agent_id_53d" varchar(50) NULL,
	"delevery_agent_addr_53d" varchar(150) NULL,
	"delevery_agent_id_53j" varchar(250) NULL,
	"intermediary_2_id_86a" varchar(50) NULL,
	"intermediary_2_code_86a" varchar(50) NULL,
	"intermediary_2_id_86d" varchar(50) NULL,
	"intermediary_2_addr_86d" varchar(150) NULL,
	"intermediary_2_id_86j" varchar(250) NULL,
	"intermediary_id_56a" varchar(50) NULL,
	"intermediary_code_56a" varchar(50) NULL,
	"intermediary_id_56d" varchar(50) NULL,
	"intermediary_addr_56d" varchar(150) NULL,
	"intermediary_id_56j" varchar(250) NULL,
	"receiving_agent_id_57a" varchar(50) NULL,
	"receiving_agent_code_57a" varchar(50) NULL,
	"receiving_agent_id_57d" varchar(50) NULL,
	"receiving_agent_addr_57d" varchar(150) NULL,
	"receiving_agent_id_57j" varchar(250) NULL,
	"beneficiary_inst_id_58a" varchar(50) NULL,
	"beneficiary_inst_code_58a" varchar(50) NULL,
	"beneficiary_inst_id_58d" varchar(50) NULL,
	"beneficiary_inst_addr_58d" varchar(150) NULL,
	"beneficiary_inst_id_58j" varchar(250) NULL,
);
CREATE_INDEX IDX_UNIQUE_CLUSTERED MT320_SequenceF_idx MT320_SequenceF  [reference_id] 

CREATE TABLE "MT320_SequenceG" (
	"reference_id" BIGINT NOT NULL,
	"tax_rate_37l" float NULL,
	"transaction_currency_33b" varchar(50) NULL,
	"transaction_net_interest_amount_33b" float NULL,
	"exchange_rate" float NULL,
	"reporting_currency_33e" varchar(50) NULL,
	"reporting_tax_amount_33e" float NULL,
);
CREATE_INDEX IDX_UNIQUE_CLUSTERED MT320_SequenceG_idx MT320_SequenceG  [reference_id] 

CREATE TABLE "MT320_SequenceH" (
	"reference_id" BIGINT NOT NULL,
	"contact_information_29a" varchar(200) NULL,
	"dealing_method_24d" varchar(50) NULL,
	"dealing_method_information_24d" varchar(50) NULL,
	"dealing_branch_party_a_id_84a" varchar(50) NULL,
	"dealing_branch_party_a_code_84a" varchar(50) NULL,
	"dealing_branch_party_a_id_84b" varchar(50) NULL,
	"dealing_branch_party_a_loc_84b" varchar(50) NULL,
	"dealing_branch_party_a_id_84d" varchar(50) NULL,
	"dealing_branch_party_a_addr_84d" varchar(200) NULL,
	"dealing_branch_party_a_id_84j" varchar(250) NULL,
	"dealing_branch_party_b_id_85a" varchar(50) NULL,
	"dealing_branch_party_b_code_85a" varchar(50) NULL,
	"dealing_branch_party_b_id_85b" varchar(50) NULL,
	"dealing_branch_party_b_loc_85b" varchar(50) NULL,
	"dealing_branch_party_b_id_85d" varchar(50) NULL,
	"dealing_branch_party_b_addr_85d" varchar(200) NULL,
	"dealing_branch_party_b_id_85j" varchar(250) NULL,
	"broker_identification_id_88a" varchar(50) NULL,
	"broker_identification_code_88a" varchar(50) NULL,
	"broker_identification_id_88d" varchar(50) NULL,
	"broker_identification_addr_88d" varchar(150) NULL,
	"broker_identification_id_88j" varchar(250) NULL,
	"send_receive_information_72" varchar(250) NULL,
);
CREATE_INDEX IDX_UNIQUE_CLUSTERED MT320_SequenceH_idx MT320_SequenceH  [reference_id] 

CREATE TABLE "MT320_CommissionFees" (
	"reference_id" BIGINT NOT NULL,
	"commission_type_34c" varchar(50) NULL,
	"currency_percent_34c" varchar(50) NULL,
	"amount_rate_34c" float NULL,
);
CREATE_INDEX IDX_UNIQUE_CLUSTERED MT320_CommissionFees_idx MT320_CommissionFees  [reference_id] 

CREATE TABLE "MT320_SequenceI" (
	"reference_id" BIGINT NOT NULL,
	"number_of_repetitions_18a" int NULL,
	"delevery_agent_id_53a" varchar(50) NULL,
	"delevery_agent_code_53a" varchar(50) NULL,
	"delevery_agent_id_53d" varchar(50) NULL,
	"delevery_agent_addr_53d" varchar(150) NULL,
	"delevery_agent_id_53j" varchar(250) NULL,
	"intermediary_2_id_86a" varchar(50) NULL,
	"intermediary_2_code_86a" varchar(50) NULL,
	"intermediary_2_id_86d" varchar(50) NULL,
	"intermediary_2_addr_86d" varchar(150) NULL,
	"intermediary_2_id_86j" varchar(250) NULL,
	"intermediary_id_56a" varchar(50) NULL,
	"intermediary_code_56a" varchar(50) NULL,
	"intermediary_id_56d" varchar(50) NULL,
	"intermediary_addr_56d" varchar(150) NULL,
	"intermediary_id_56j" varchar(250) NULL,
	"receiving_agent_id_57a" varchar(50) NULL,
	"receiving_agent_code_57a" varchar(50) NULL,
	"receiving_agent_id_57d" varchar(50) NULL,
	"receiving_agent_addr_57d" varchar(150) NULL,
	"receiving_agent_id_57j" varchar(250) NULL,
);
CREATE_INDEX IDX_UNIQUE_CLUSTERED MT320_SequenceI_idx MT320_SequenceI  [reference_id] 

CREATE TABLE "MT320_AdditionalAmounts" (
	"reference_id" BIGINT NOT NULL,
	"payment_date_30F" date NULL,
	"currency_32h" varchar(50) NULL,
	"payment_amount_32h" float NULL,
);
CREATE_INDEX IDX_UNIQUE_CLUSTERED MT320_AdditionalAmounts_idx MT320_AdditionalAmounts  [reference_id]













