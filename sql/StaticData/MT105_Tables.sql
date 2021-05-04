CREATE TABLE "MT105_Block1" (
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
CREATE UNIQUE INDEX MT105_Block1_idx ON MT105_Block1 (reference_id);

CREATE TABLE "MT105_Block2" (
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
CREATE UNIQUE INDEX MT105_Block2_idx ON MT105_Block2 (reference_id);

CREATE TABLE "MT105_Block3" (
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
CREATE UNIQUE INDEX MT105_Block3_idx ON MT105_Block3 (reference_id);

CREATE TABLE "MT105_Block5" (
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
CREATE UNIQUE INDEX MT105_Block5_idx ON MT105_Block5 (reference_id);

CREATE TABLE "MT105_SequenceA" (
	"reference_id" BIGINT NOT NULL,
	"message_number_27" varchar(50) NULL,
	"sequence_number_27" varchar(50) NULL,
	"transaction_reference_number_20" varchar(50) NULL,
	"related_reference_21" varchar(50) NULL,
	"sub_message_type_12" varchar(50) NULL,
	"edifact_message_77f" varchar(1800) NULL,
);
CREATE UNIQUE INDEX MT105_SequenceA_idx ON MT105_SequenceA (reference_id);
