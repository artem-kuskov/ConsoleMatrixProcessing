# ConsoleMatrixProcessing
## utility for batch matrix processing.

Command line parameters:

**ConsoleMatrixProcessing    {*path* | help | -help | /?} [parallelism=*number*] [buffer=*number*]**

Where:

**path**:                  Path to the directory with files contained matrix operation data. 
Result files will be saved to the same path.
                
**help**:                  Command to show this help info.
    
**parallelism**=*number*:  Maximum number of parallel threads using for processing. Default parallelism=4.
    
**buffer**=*number*:       Maximum number of batches in memory. Default buffer=100.



## Specification:
>--------------
>Написать консольное приложение, выполняющее операции над матрицами: сложение, вычитание, умножение, транспонирование
>
>На вход приложение получает имядиректории с текстовыми файлами. Файлов может быть много.
>
>Файл имеетследующий формат:
>
>1я строка - слово означающее, что нужно сделать (multiply,add, subtract, transpose). Затем пустая строка. Далее матрицы(а), над которыминужно провести операцию. Числа в строке матрицы разделены пробелами, матрицыдруг от друго отделены пустой строкой.
>
>Пример содержимогофайла:
>
    —---------------------------------—
    
    multiply
    
    
    2 5 6 99
    
    8 55 6 9
    
    7 8 5 56
    
    
    59 48 65
    
    59 141 56
    
    5 5 6
    
    —---------------------------------—
>
>
>Врезультате умножения, сложения, вычитания должна получиться 1 матрица. Врезультате транспонирования - столько же сколько и во входном файле.
>
>Врезультате работы программа должна сохранить в исходную папку файлы с ответами, по одному на каждый входной файл (<имяиходногофайла>_result.txt).
>
>Программа должна отображать текущийпрогресс.

## Сlarification
1. Содержимое матриц - целые числа int, как в примере, или могут быть вещественные(double)?

>На усмотрение кандидата. Не существенно.

2. Подразуумевается, что результаты вычислений матриц укладываются в тот же тип данных, из которых матрица состоит? (т.е. например, при умножении матриц считаем, что результаты не должны переполнять тот же int или double?)

>На усмотрение кандидата. Вопрос правильный.

3. Для операции вычитания, когда в файле задано больше двух матриц, мы выполняем операцию (((А - B) - C) - D) (где A,BC,D - матрицы в файле в порядке их следования)? Т.е. из первой матрицы вычитается вторая, из полученного результата вычитается следующая и т.д.?

>Именно так.

А для операций умножения выполняем операцию (((А х B) х C) х D) ? Т.е. первую умножаем на вторую и полученный результат умножаем на следующую матрицу и т.д?

>Именно так.

В примере операции умножения, который указан в задании, невозможно умножить первую матрицу на вторую, но можно вторую умножить на первую. Это ошибка в задании или подразуумевается обратный порядок следования матриц?

>Это ошибка. Спасибо, что заметили. Исправим.

## Сlarification Summary
It is assumed that matrices contain Int32 integer data and the result of matrix calculation is also an Int32 integer type.

If data cannot be deserialized to matrices, an error is generated. 

The calculation checks for overflow. In case of overflow, an error is generated.

If the source folder contains files that are not command files, those files are skipped.

## Architecture
The project contains three layers:

* **Core** - classes that perform the main business tasks of the project-matrix calculations. 
* **Application** - classes that transform and transfer data between the Core and Services layers.
* **Services** - easily replaceable classes that work with low-level data stores and cross-boarding functionality.

The project uses Dependency Injection to provide Core and Application with replaceable lower layers interface implementations.

Core has static class ProcessorCommandFabric to produce different implementation classes for IProcessorCommand interface.

Serialize and Deserialize operations implemented in Application layer as extensions methods for data models.

## Conveyor
Startup class orginizes and orchestrates DataFlow conveyor, where different data transformations are linked together via data buffers.

Data transformers that performs calculations tasks work in parallel.

Buffers size and parallelism are confugurable.

## Test
Project ConsoleMatrixProcessing.Tests has several unit tests for example.

Also there is instructions and test files in ConsoleMatrixProcessing.Tests/Manual.Tests for manual testing.