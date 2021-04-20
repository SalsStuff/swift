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
)
;
create unique index MT320_Block1_idx on MT320_Block1 (reference_id);

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
	"mir_sender_date" VARCHAR(50) NULL,
	"mir_lt_address" VARCHAR(50) NULL,
	"mir_bic_code" VARCHAR(50) NULL,
	"mir_lt_code" VARCHAR(50) NULL,
	"mir_bic_branch_code" VARCHAR(50) NULL,
	"mir_session_number" VARCHAR(50) NULL,
	"mir_sequence_number" VARCHAR(50) NULL,
	"output_date" VARCHAR(50) NULL,
	"output_time" VARCHAR(50) NULL,
)
;
create unique index MT320_Block2_idx on MT320_Block2 (reference_id);

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
	"tag121_unique_tran_reference" VARCHAR(50) NULL,
	"tag115_adressee_info" VARCHAR(50) NULL,
	"tag165_payment_rir" VARCHAR(50) NULL,
	"tag433_sanctions_sir" VARCHAR(50) NULL,
	"tag434_payment_cir" VARCHAR(50) NULL,
)
;
create unique index MT320_Block3_idx on MT320_Block3 (reference_id);

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
	"pde_mir_sequence_number" VARCHAR(50) NULL,
	"dlm" VARCHAR(50) NULL,
	"mrf" VARCHAR(50) NULL,
	"mrf_date" VARCHAR(50) NULL,
	"mrf_time" VARCHAR(50) NULL,
	"mrf_mir" VARCHAR(50) NULL,
	"pdm" VARCHAR(50) NULL,
	"pdm_time" VARCHAR(50) NULL,
	"pdm_mor" VARCHAR(50) NULL,
	"pdm_mor_date" VARCHAR(50) NULL,
	"pdm_mor_lt_id" VARCHAR(50) NULL,
	"pdm_mor_session_number" VARCHAR(50) NULL,
	"pdm_mor_sequence_number" VARCHAR(50) NULL,
	"sys" VARCHAR(50) NULL,
	"sys_time" VARCHAR(50) NULL,
	"sys_mor" VARCHAR(50) NULL,
	"sys_mor_date" VARCHAR(50) NULL,
	"sys_mor_lt_id" VARCHAR(50) NULL,
	"sys_mor_session_number" VARCHAR(50) NULL,
	"sys_mor_sequence_number" VARCHAR(50) NULL,
)
;
create unique index MT320_Block5_idx on MT320_Block5 (reference_id);