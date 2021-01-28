using System;
using System.Collections.Generic;
using System.Collections;


namespace MyProject
{
    class Encoder
    {
        private int[] array = new int[30000];
        private int size;
        private int nrow;
        private int ncol;
        private int dataCodes;
        private int RSCodes;
        private int dataSize;
       
        private int[] COLUMNS =
        {
            10, 12, 14, 16, 18, 20, 22, 24, 26, 32, 36, 40, 44, 48, 52, 64, 72, 80, 88, 96, 104, 120, 132, 144, 18, 32, 26, 36, 36, 48
        };
        private int[] ROWS =
        {
            10, 12, 14, 16, 18, 20, 22, 24, 26, 32, 36, 40, 44, 48, 52, 64, 72, 80, 88, 96, 104, 120, 132, 144, 8, 8, 12, 12, 16, 16
        };
        // количество кодов данных в зависимости от матрицы
        private int[] DATA_CODES =
        {
            3, 5, 8, 12, 18, 22, 30, 36, 44, 62, 86, 114, 144, 174, 204, 280, 368, 456, 576, 696, 816, 1050, 1304, 1558, 5, 10, 16, 22,
            32, 49
        };
        // количество кодов коррекции в зависимости от матрицы
        private int[] ERROR_CODES =
        {
            5, 7, 10, 12, 14, 18, 20, 24, 28, 36, 42, 48, 56, 68, 42, 56, 36, 48, 56, 68, 56, 68, 62, 62, 7, 11, 14, 18, 24, 28
        };

        private int[] REGION_COLUMNS = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 4, 4, 4, 4, 4, 4, 6, 6, 6, 1, 2, 1, 2, 2, 2 };

        private int[] REGION_ROWS = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 4, 4, 4, 4, 4, 4, 6, 6, 6, 1, 1, 1, 1, 1, 1 };

        private int[] BLOCKS =
        {
            1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 4, 4, 4, 4, 6, 6, 8, 10, 1, 1, 1, 1, 1, 1
        };

        public int GetColumns()
        {
            return COLUMNS[size];
        }
        public int GetRows()
        {
            return ROWS[size];
        }
        private bool[] GetFullMatrix()
        {
            var rows = ROWS[size];
            var columns = COLUMNS[size];
            var regionColumns = columns / REGION_COLUMNS[size];
            var regionRows = rows / REGION_ROWS[size];
            var matrix = new bool[columns * rows];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {

                    if (i % regionRows == 0)
                    {
                        matrix[i * columns + j] = !Convert.ToBoolean(j % 2);
                        continue;
                    }
                    if (((i + 1) % regionRows == 0) || (j % regionColumns == 0))
                    {
                        matrix[i * columns + j] = true;
                        continue;
                    }
                    if ((j + 1) % regionColumns == 0)
                    {
                        matrix[i * columns + j] = Convert.ToBoolean(i % 2);
                        continue;
                    }
                    int row = i, column = j;
                    row -= (1 + (row / regionRows) * 2);
                    column -= (1 + (column / regionColumns) * 2);
                    matrix[i * columns + j] = Convert.ToBoolean(array[row * ncol + column]);
                }
            }
            return matrix;
        }
        // точка входа класса, в ней мы вызываем необходимые функции
        public bool[] Encode(string text)
        {
            var data = new List<int>();
            EncodeText(text, data);
            RSCodes = ERROR_CODES[size];
            nrow = ROWS[size] - REGION_ROWS[size] * 2;
            ncol = COLUMNS[size] - REGION_COLUMNS[size] * 2;
            CreateSolomonCode(data);
            MapDataMatrix();
            Fill(data);
            bool[] matrix = GetFullMatrix();
             return matrix;
        }
        // Умножение чисел в поле Галуа
        private int Mult(int[] log, int[] alog, int a, int b)
        {
            if (a == 0 || b == 0)
            {
                return 0;
            }
            return alog[(log[a] + log[b]) % 255];
        }
        private void CreateSolomonCode(List<int> data)
        {
           
            // log[i] = log(i) по основанию 2
            var log = new int[256];
            // операция обратная log - alog(log(i)) = i = 2^(log(i))
            var alog = new int[256];
            alog[0] = 1;
            log[1] = 0;
            // Подсчитываем степени двойки в поле Галуа
            for (int i = 1; i < 256; i++)
            {
                int value = 2 * alog[i - 1];
                if (value >= 256)
                {
                    alog[i] = value ^ 301;
                }
                else
                {
                    alog[i] = value;
                }
                log[alog[i]] = i;
            }
       
            // На этом шаге вычисляем члены порождающего полинома
            var poly = new int[RSCodes + 1];
            poly[0] = 1;
            for (var i = 1; i <= RSCodes; i++)
            {
                poly[i] = poly[i - 1];
                for (var j = i - 1; j >= 1; j--)
                {
                    poly[j] = poly[j - 1] ^ Mult(log, alog, poly[j], alog[i]);
                }
                poly[0] = Mult(log, alog, poly[0], alog[i]);
            }

            // Получаем значение кодов коррекции и заносим их к общим данным
            
            var blocks = BLOCKS[size];
            dataSize = dataCodes + blocks * RSCodes;
            var wd = new int[10000];
            for (int k = 0; k < blocks; k++)
            {
                for (int i = k; i < dataCodes; i += blocks)
                {
                    
                    var el = wd[k] ^ data[i];
                    int j = k;
                    for (int cnt = 0; RSCodes - 1 -cnt >= 0;  j += blocks)
                    {
            
                        if (j + blocks < dataSize)
                        {
                            wd[j] = wd[j + blocks] ^ Mult(log, alog, el, poly[RSCodes - cnt - 1]);
                        } else
                        {
                            wd[j] = Mult(log, alog, el, poly[RSCodes - cnt - 1]);
                        }
                        cnt++;
                    }
                }
            }

            for (int i = 0; i < RSCodes * blocks; i++)
            {
                data.Add(wd[i]);
            }
        }
        // Обход матрицы и заполнение её числами вида C*10 + bit, C- порядковый номер кода, bit-номер бита кода
        private void MapDataMatrix()
        {
            
            var c = 1;
            var row = 4;
            var col = 0;
            while (true)
            {
                //обработка особых случаев
                if (row == nrow && col == 0)
                {
                    Corner1(c);
                    c++;
                }
                if (row == nrow - 2 && col == 0 && (ncol % 4) != 0)
                {
                    Corner2(c);
                    c++;
                }
                if ((row == nrow - 2) && (col == 0) && (ncol % 8 == 4))
                {
                    Corner3(c);
                    c++;
                }

                if ((row == nrow + 4) && (col == 2) && (ncol % 8) == 0)
                {
                    Corner4(c);
                    c++;
                }
                //заполнение матрицы обычными формами
                while (true)
                {
                    if ((row < nrow) && (col >= 0) && array[row * ncol + col] == 0)
                    {
                        SimpleCase(row, col, c);
                        c++;
                    }
                    row -= 2;
                    col += 2;
                    if (!(row >= 0 && col < ncol))
                    {
                        break;
                    }
                }
                row++;
                col += 3;
                while (true)
                {
                    if ((row >= 0) && (col < ncol) && array[row * ncol + col] == 0)
                    {
                        SimpleCase(row, col, c);
                        c++;
                    }
                    row += 2;
                    col -= 2;
                    if (!(row < nrow && col >= 0))
                    {
                        break;
                    }
                }
                row += 3;
                col++;
                if (!((row < nrow) || col < ncol))
                {
                    break;
                }
            }
            // случай когда в нижнем правом углу есть незаполненный квадрат
            if (array[nrow * ncol - 2] == 0)
            {
                array[nrow * ncol - ncol - 2] = 1;
                array[nrow * ncol - ncol - 1] = 0;
                array[nrow * ncol - 2] = 0;
                array[nrow * ncol - 1] = 1;
            }
        }

        private int Find(int[] data, int elem)
        {
            for (int i = 0; i < nrow * ncol; i++)
            {
                if (data[i] == elem)
                {
                    return i;
                } 
            }
            return -1;
        }
        // заполняем матрицу нулями и единицами
        private void Fill(List<int> data)
        {
            var a = new int[ncol * nrow];
            for (int i = 0; i < nrow * ncol; i++)
            {
                a[i] = array[i];
            }

            for (int i = 0; i < data.Count; i++)
            {
               var v = data[i];
               for (int j = 7; j >= 0; j--)
                {
                    var kk = 10 * (i + 1) + 8 - j;
                    var k = Find(a, kk);
                    int res = (1 << j);
                    if (v >= res)
                    {
                        v -= res;
                        a[k] = 1;
                    } else
                    {
                        a[k] = 0;
                    }
                }
            }
            for (int i = 0; i < nrow * ncol; i++)
            {
                array[i] = a[i];
            }

        }
        // помечаем заданный элемент матрицы
        private void SetModule(int row, int col, int c, int bit)
        {
            if (row < 0)
            {
                row += nrow;
                col += 4 - ((nrow + 4) % 8);
            }
            if (col < 0)
            {
                col += ncol;
                row += 4 - ((ncol + 4) % 8);
            }
            array[row * ncol + col] = 10 * c + bit;
        }

        private void Corner1(int c)
        {
            SetModule(nrow - 1, 0, c, 1);

            SetModule(nrow - 1, 1, c, 2);

            SetModule(nrow - 1, 2, c, 3);

            SetModule(0, ncol - 2, c, 4);

            SetModule(0, ncol - 1, c, 5);

            SetModule(1, ncol - 1, c, 6);

            SetModule(2, ncol - 1, c, 7);

            SetModule(3, ncol - 1, c, 8);
        }
        private void Corner2(int c)
        {
            SetModule(nrow - 3, 0, c, 1);

            SetModule(nrow - 2, 0, c, 2);

            SetModule(nrow - 1, 0, c, 3);

            SetModule(0, ncol - 4, c, 4);

            SetModule(0, ncol - 3, c, 5);

            SetModule(0, ncol - 2, c, 6);

            SetModule(0, ncol - 1, c, 7);

            SetModule(1, ncol - 1, c, 8);
        }
        private void Corner3(int c)
        {
            SetModule(nrow - 3, 0, c, 1);

            SetModule(nrow - 2, 0, c, 2);

            SetModule(nrow - 1, 0, c, 3);

            SetModule(0, ncol - 2, c, 4);

            SetModule(0, ncol - 1, c, 5);

            SetModule(1, ncol - 1, c, 6);

            SetModule(2, ncol - 1, c, 7);

            SetModule(3, ncol - 1, c, 8);
        }
        private void Corner4(int c)
        {
            SetModule(nrow - 1, 0, c, 1);

            SetModule(nrow - 1, ncol - 1, c, 2);

            SetModule(0, ncol - 3, c, 3);

            SetModule(0, ncol - 2, c, 4);

            SetModule(0, ncol - 1, c, 5);

            SetModule(1, ncol - 3, c, 6);

            SetModule(1, ncol - 2, c, 7);

            SetModule(1, ncol - 1, c, 8);
        }


        private void SimpleCase(int row, int col, int c)
        {
            SetModule(row - 2, col - 2, c, 1);

            SetModule(row - 2, col - 1, c, 2);

            SetModule(row - 1, col - 2, c, 3);

            SetModule(row - 1, col - 1, c, 4);

            SetModule(row - 1, col, c, 5);

            SetModule(row, col - 2, c, 6);

            SetModule(row, col - 1, c, 7);

            SetModule(row, col, c, 8);
        }
        // получаем size, от размера которого зависят значения заданных констант
        private void SetSize(int wordCount)
        {
            for (var i = 0; i < 30; i++)
            {
                if (DATA_CODES[i] >= wordCount)
                {
                    size = i;
                    return;
                }
            }
        }
        private  void EncodeText (string text, List<int> data)
        {
            var length = text.Length;
            for (int i = 0; i < length; i++)
            {
                var character = text[i];
                // Поддерживается кодировка только 256 символов(8 бит на символ)
                if (character >= 256)
                {
                    throw new Exception("Не поддерживается символ");
                }
                //Если это не Ascii код
                if (character >= 128)
                {
                    data.Add(235);
                    data.Add(character - 127);
                    continue;
                }
                //особое правило для подряд идущих цифр
                if (i + 1 < length && character >= 48 && character <= 57 && text[i + 1] >= 48 && text[i + 1] <= 57)
                {
                    i++;
                    data.Add(130 + (character - 48) * 10 + text[i] - 48);
                    continue;
                }
                data.Add(character + 1);
            }
            //Создаем коды отступов, если количество кодов символов меньше необходимого 
            SetSize(data.Count);
            dataCodes = DATA_CODES[size];
            if (data.Count < dataCodes)
            {
                data.Add(129);
                while (data.Count < dataCodes)
                {
                    int r = ((149 * (data.Count + 1)) % 253) + 1;
                    data.Add(((129 + r) % 254));
                }
            }
          
        }
    }
}
