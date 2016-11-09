using System.Text;

namespace ExcelToolKit.BinaryFormat {
    /// <summary>
    /// Represents Sheet record in Workbook Globals
    /// </summary>
    internal class XlsBiffBoundSheet : XlsBiffRecord {
        #region SheetType enum

        public enum SheetType : byte {
            Worksheet=0x0,
            MacroSheet=0x1,
            Chart=0x2,
            VBModule=0x6
        }

        #endregion

        #region SheetVisibility enum

        public enum SheetVisibility : byte {
            Visible=0x0,
            Hidden=0x1,
            VeryHidden=0x2
        }

        #endregion

        private bool isV8=true;
        private Encoding m_UseEncoding=ExcelToolKit.Extension.DefaultEncoding();

        internal XlsBiffBoundSheet(byte[] bytes, uint offset, ExcelBinaryReader reader)
            : base(bytes, offset, reader) {
        }

        /// <summary>
        /// Worksheet data start offset
        /// </summary>
        public uint StartOffset {
            get {
                return base.ReadUInt32(0x0);
            }
        }

        /// <summary>
        /// Type of worksheet
        /// </summary>
        public SheetType Type {
            get {
                return (SheetType)base.ReadByte(0x4);
            }
        }

        /// <summary>
        /// Visibility of worksheet
        /// </summary>
        public SheetVisibility VisibleState {
            get {
                return (SheetVisibility)(base.ReadByte(0x5)&0x3);
            }
        }

        /// <summary>
        /// Name of worksheet
        /// </summary>
        public string SheetName {
            get {
                ushort len=base.ReadByte(0x6);

                const int start = 0x8;
                if (isV8) {
                    if (base.ReadByte(0x7)==0) {
                        return ExcelToolKit.Extension.DefaultEncoding().GetString(m_bytes, m_readoffset+start, len);
                    } else {
                        return m_UseEncoding.GetString(m_bytes, m_readoffset+start,
                                                       m_UseEncoding.IsSingleByteEncoding()?len:len*2);
                    }
                } else {
                    return ExcelToolKit.Extension.DefaultEncoding().GetString(m_bytes, m_readoffset+start-1, len);
                }
            }
        }

        /// <summary>
        /// Encoding used to deal with strings
        /// </summary>
        public Encoding UseEncoding {
            get {
                return m_UseEncoding;
            }
            set {
                m_UseEncoding=value;
            }
        }

        /// <summary>
        /// Specifies if BIFF8 format should be used
        /// </summary>
        public bool IsV8 {
            get {
                return isV8;
            }
            set {
                isV8=value;
            }
        }
    }
}