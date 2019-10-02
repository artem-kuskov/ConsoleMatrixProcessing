# Instruction to run manual test


> ## ! New  integration test added to Integration.Tests/ProgramTests.cs that run this manual test in automat mode !


1. Copy folder "test_data" to temporary folder <temp>.
2. Build and publish project ConsoleMatrixProcessing as executable file in temporary folder <temp>
3. Open command line interpreter and run command:
* Windows:
>		cd <temp>
>		ConsoleMatrixProcessing help

* Linux:
>		cd <temp>
>		./ConsoleMatrixProcessing help
4. Exspected result in console:
>	This is utility for batch matrix processing.
>	Command line parameters:
>		{<path> | help | -help | /?} [parallelism=<number>] [buffer=<number>]

>	Where:
>		**<path>**:                 Path to the directory with files contained matrix operation data.
>								Result files will be saved to the same path.
>		**help**:                   Command to show this help info.
>		**parallelism**=*<number>*:   Maximum number of parallel threads using for processing. Default parallelism=4.
>		**buffer**=*<number>*:        Maximum number of batches in memory. Default buffer=100.

5. Run command:
* Windows:
>		ConsoleMatrixProcessing <test_data>
 
* Linux:
>		./ConsoleMatrixProcessing <test_data>
6. Exspected result in console:
>		<date time> [Information] Start process with parallelism=4
>		<date time> [Information] Observed path: "<temp>\test_data"
>		<date time> [Information] Read file "<temp>\test_data\add1_good.txt"
>		<date time> [Information] Read file "<temp>\test_data\add2_error_format.txt"
>		<date time> [Information] Read file "<temp>\test_data\add3_error_data.txt"
>		<date time> [Information] Read file "<temp>\test_data\add4_good.txt"
>		<date time> [Information] Read file "<temp>\test_data\add5_error_overflow.txt"
>		<date time> [Information] Read file "<temp>\test_data\mult1_good.txt"
>		<date time> [Information] Read file "<temp>\test_data\mult2_error_format.txt"
>		<date time> [Information] Read file "<temp>\test_data\mult3_error_data.txt"
>		<date time> [Information] Read file "<temp>\test_data\mult4_good.txt"
>		<date time> [Information] Read file "<temp>\test_data\mult5_error_overflow.txt"
>		<date time> [Information] Read file "<temp>\test_data\sub1_good.txt"
>		<date time> [Information] Read file "<temp>\test_data\sub2_error_format.txt"
>		<date time> [Information] Read file "<temp>\test_data\sub3_error_data.txt"
>		<date time> [Information] Read file "<temp>\test_data\sub4_good.txt"
>		<date time> [Information] Read file "<temp>\test_data\sub5_error_overflow.txt"
>		<date time> [Information] Read file "<temp>\test_data\tran1_good.txt"
>		<date time> [Information] Read file "<temp>\test_data\tran2_error_format.txt"
>		<date time> [Information] Read file "<temp>\test_data\tran3_error_format.txt"
>		<date time> [Information] Read file "<temp>\test_data\tran4_skip_notcommand.txt"
>		<date time> [Error] Data format error in "<temp>\test_data\add2_error_format.txt"
>		<date time> [Error] Error processing command in "<temp>\test_data\add3_error_data.txt" ("Adding matrices have different size in data source <temp>\test_data\add3_error_data.txt")
>		<date time> [Error] Data format error in "<temp>\test_data\mult2_error_format.txt"
>		<date time> [Error] Error processing command in "<temp>\test_data\add5_error_overflow.txt" ("Arithmetic operation resulted in an overflow.")
>		<date time> [Error] Error processing command in "<temp>\test_data\mult3_error_data.txt" ("Left matrix must has the same number of columns as right matrix has rows in data source <temp>\test_data\mult3_error_data.txt")
>		<date time> [Error] Data format error in "<temp>\test_data\sub2_error_format.txt"
>		<date time> [Error] Error processing command in "<temp>\test_data\mult5_error_overflow.txt" ("Left matrix must has the same number of columns as right matrix has rows in data source <temp>\test_data\mult5_error_overflow.txt")
>		<date time> [Error] Error processing command in "<temp>\test_data\sub3_error_data.txt" ("Subtracking matrices have different size in data source <temp>\test_data\sub3_error_data.txt")
>		<date time> [Error] Error processing command in "<temp>\test_data\sub5_error_overflow.txt" ("Arithmetic operation resulted in an overflow.")
>		<date time> [Error] Data format error in "<temp>\test_data\tran2_error_format.txt"
>		<date time> [Error] Data format error in "<temp>\test_data\tran3_error_format.txt"
>		<date time> [Information] Write result to file "<temp>\test_data\add1_good.txt_result.txt"
>		<date time> [Information] Write result to file "<temp>\test_data\add4_good.txt_result.txt"
>		<date time> [Information] Write result to file "<temp>\test_data\mult1_good.txt_result.txt"
>		<date time> [Information] Write result to file "<temp>\test_data\mult4_good.txt_result.txt"
>		<date time> [Information] Write result to file "<temp>\test_data\sub1_good.txt_result.txt"
>		<date time> [Information] Write result to file "<temp>\test_data\sub4_good.txt_result.txt"
>		<date time> [Information] Write result to file "<temp>\test_data\tran1_good.txt_result.txt"
>		<date time> [Information] End of processing
>		<date time> [Information] Files found: 19. Files read: 19. Commands found: 13. Commands successfully calculated: 7. Files saved: 7. Time elapsed: <> milliseconds

5. Files in *<temp>/test_data* must be equal to reference files in *reference_result* folder.