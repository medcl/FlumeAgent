/**
 * Autogenerated by Thrift
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using Thrift.Protocol;
using Thrift.Transport;

[Serializable]
internal partial class ThriftFlumeEvent : TBase
{
  private long _timestamp;
  private Priority _priority;
  private byte[] _body;
  private long _nanos;
  private string _host;
  private Dictionary<string, byte[]> _fields;

  public long Timestamp
  {
    get
    {
      return _timestamp;
    }
    set
    {
      __isset.timestamp = true;
      this._timestamp = value;
    }
  }

  public Priority Priority
  {
    get
    {
      return _priority;
    }
    set
    {
      __isset.priority = true;
      this._priority = value;
    }
  }

  public byte[] Body
  {
    get
    {
      return _body;
    }
    set
    {
      __isset.body = true;
      this._body = value;
    }
  }

  public long Nanos
  {
    get
    {
      return _nanos;
    }
    set
    {
      __isset.nanos = true;
      this._nanos = value;
    }
  }

  public string Host
  {
    get
    {
      return _host;
    }
    set
    {
      __isset.host = true;
      this._host = value;
    }
  }

  public Dictionary<string, byte[]> Fields
  {
    get
    {
      return _fields;
    }
    set
    {
      __isset.fields = true;
      this._fields = value;
    }
  }


  internal Isset __isset;
  [Serializable]
  internal struct Isset
  {
    public bool timestamp;
    public bool priority;
    public bool body;
    public bool nanos;
    public bool host;
    public bool fields;
  }

  public ThriftFlumeEvent() {
  }

  public void Read (TProtocol iprot)
  {
    TField field;
    iprot.ReadStructBegin();
    while (true)
    {
      field = iprot.ReadFieldBegin();
      if (field.Type == TType.Stop) { 
        break;
      }
      switch (field.ID)
      {
        case 1:
          if (field.Type == TType.I64) {
            Timestamp = iprot.ReadI64();
          } else { 
            TProtocolUtil.Skip(iprot, field.Type);
          }
          break;
        case 2:
          if (field.Type == TType.I32) {
            Priority = (Priority)iprot.ReadI32();
          } else { 
            TProtocolUtil.Skip(iprot, field.Type);
          }
          break;
        case 3:
          if (field.Type == TType.String) {
            Body = iprot.ReadBinary();
          } else { 
            TProtocolUtil.Skip(iprot, field.Type);
          }
          break;
        case 4:
          if (field.Type == TType.I64) {
            Nanos = iprot.ReadI64();
          } else { 
            TProtocolUtil.Skip(iprot, field.Type);
          }
          break;
        case 5:
          if (field.Type == TType.String) {
            Host = iprot.ReadString();
          } else { 
            TProtocolUtil.Skip(iprot, field.Type);
          }
          break;
        case 6:
          if (field.Type == TType.Map) {
            {
              Fields = new Dictionary<string, byte[]>();
              TMap _map0 = iprot.ReadMapBegin();
              for( int _i1 = 0; _i1 < _map0.Count; ++_i1)
              {
                string _key2;
                byte[] _val3;
                _key2 = iprot.ReadString();
                _val3 = iprot.ReadBinary();
                Fields[_key2] = _val3;
              }
              iprot.ReadMapEnd();
            }
          } else { 
            TProtocolUtil.Skip(iprot, field.Type);
          }
          break;
        default: 
          TProtocolUtil.Skip(iprot, field.Type);
          break;
      }
      iprot.ReadFieldEnd();
    }
    iprot.ReadStructEnd();
  }

  public void Write(TProtocol oprot) {
    TStruct struc = new TStruct("ThriftFlumeEvent");
    oprot.WriteStructBegin(struc);
    TField field = new TField();
    if (__isset.timestamp) {
      field.Name = "timestamp";
      field.Type = TType.I64;
      field.ID = 1;
      oprot.WriteFieldBegin(field);
      oprot.WriteI64(Timestamp);
      oprot.WriteFieldEnd();
    }
    if (__isset.priority) {
      field.Name = "priority";
      field.Type = TType.I32;
      field.ID = 2;
      oprot.WriteFieldBegin(field);
      oprot.WriteI32((int)Priority);
      oprot.WriteFieldEnd();
    }
    if (Body != null && __isset.body) {
      field.Name = "body";
      field.Type = TType.String;
      field.ID = 3;
      oprot.WriteFieldBegin(field);
      oprot.WriteBinary(Body);
      oprot.WriteFieldEnd();
    }
    if (__isset.nanos) {
      field.Name = "nanos";
      field.Type = TType.I64;
      field.ID = 4;
      oprot.WriteFieldBegin(field);
      oprot.WriteI64(Nanos);
      oprot.WriteFieldEnd();
    }
    if (Host != null && __isset.host) {
      field.Name = "host";
      field.Type = TType.String;
      field.ID = 5;
      oprot.WriteFieldBegin(field);
      oprot.WriteString(Host);
      oprot.WriteFieldEnd();
    }
    if (Fields != null && __isset.fields) {
      field.Name = "fields";
      field.Type = TType.Map;
      field.ID = 6;
      oprot.WriteFieldBegin(field);
      {
        oprot.WriteMapBegin(new TMap(TType.String, TType.String, Fields.Count));
        foreach (string _iter4 in Fields.Keys)
        {
          oprot.WriteString(_iter4);
          oprot.WriteBinary(Fields[_iter4]);
          oprot.WriteMapEnd();
        }
      }
      oprot.WriteFieldEnd();
    }
    oprot.WriteFieldStop();
    oprot.WriteStructEnd();
  }

  public override string ToString() {
    StringBuilder sb = new StringBuilder("ThriftFlumeEvent(");
    sb.Append("Timestamp: ");
    sb.Append(Timestamp);
    sb.Append(",Priority: ");
    sb.Append(Priority);
    sb.Append(",Body: ");
    sb.Append(Body);
    sb.Append(",Nanos: ");
    sb.Append(Nanos);
    sb.Append(",Host: ");
    sb.Append(Host);
    sb.Append(",Fields: ");
    sb.Append(Fields);
    sb.Append(")");
    return sb.ToString();
  }

}
