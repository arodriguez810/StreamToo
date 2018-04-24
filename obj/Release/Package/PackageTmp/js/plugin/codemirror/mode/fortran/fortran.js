// CodeMirror, copyright (c) by Marijn Haverbeke and others
// Distributed under an MIT license: http://codemirror.net/LICENSE

(function(mod) {
  if (typeof exports == "object" && typeof module == "object") // CommonJS
    mod(require("../../lib/codemirror"));
  else if (typeof define == "function" && define.amd) // AMD
    define(["../../lib/codemirror"], mod);
  else // Plain browser env
    mod(CodeMirror);
})(function(CodeMirror) {
"use strict";

CodeMirror.defineMode("fortran", function() {
  function words(array) {
    var keys = {};
    for (var i = 0; i < array.length; ++i) {
      keys[array[i]] = true;
    }
    return keys;
  }

  var keywords = words([
                  "abstract", "accept", "allocatable", "allocate",
                  "array", "assign", "asynchronous", "backspace",
                  "bind", "block", "byte", "call", "case",
                  "class", "close", "common", "contains",
                  "continue", "cycle", "data", "deallocate",
                  "decode", "deferred", "dimension", "do",
                  "elemental", "else", "encode", "end",
                  "endif", "entry", "enumerator", "equivalence",
                  "exit", "external", "extrinsic", "final",
                  "forall", "format", "function", "generic",
                  "go", "goto", "if", "implicit", "import", "include",
                  "inquire", "intent", "interface", "intrinsic",
                  "module", "namelist", "non_intrinsic",
                  "non_overridable", "none", "nopass",
                  "nullify", "open", "optional", "options",
                  "parameter", "pass", "pause", "pointer",
                  "print", "private", "program", "protected",
                  "public", "pure", "read", "recursive", "result",
                  "return", "rewind", "save", "select", "sequence",
                  "stop", "subroutine", "target", "then", "to", "type",
                  "use", "value", "volatile", "where", "while",
                  "write"]);
  var builtins = words(["abort", "abs", "access", "achar", "acos",
                          "adjustl", "adjustr", "aimag", "aint", "alarm",
                          "all", "allocated", "alog", "amax", "amin",
                          "amod", "and", "anint", "any", "asin",
                          "associated", "atan", "besj", "besjn", "besy",
                          "besyn", "bit_size", "btest", "cabs", "ccos",
                          "ceiling", "cexp", "char", "chdir", "chmod",
                          "clog", "cmplx", "command_argument_count",
                          "complex", "conjg", "cos", "cosh", "count",
                          "cpu_time", "cshift", "csin", "csqrt", "ctime",
                          "VWfunloc", "VWloc", "VWassociated", "VWnull_ptr",
                          "VWnull_funptr", "VWf_pointer", "VWnull_char",
                          "VWalert", "VWbackspace", "VWform_feed",
                          "VWnew_line", "VWcarriage_return",
                          "VWhorizontal_tab", "VWvertical_tab", "dabs",
                          "dacos", "dasin", "datan", "date_and_time",
                          "dbesj", "dbesj", "dbesjn", "dbesy", "dbesy",
                          "dbesyn", "dble", "dcos", "dcosh", "ddim", "derf",
                          "derfc", "dexp", "digits", "dim", "dint", "dlog",
                          "dlog", "dmax", "dmin", "dmod", "dnint",
                          "dot_product", "dprod", "dsign", "dsinh",
                          "dsin", "dsqrt", "dtanh", "dtan", "dtime",
                          "eoshift", "epsilon", "erf", "erfc", "etime",
                          "exit", "exp", "exponent", "extends_type_of",
                          "fdate", "fget", "fgetc", "float", "floor",
                          "flush", "fnum", "fputc", "fput", "fraction",
                          "fseek", "fstat", "ftell", "gerror", "getarg",
                          "get_command", "get_command_argument",
                          "get_environment_variable", "getcwd",
                          "getenv", "getgid", "getlog", "getpid",
                          "getuid", "gmtime", "hostnm", "huge", "iabs",
                          "iachar", "iand", "iargc", "ibclr", "ibits",
                          "ibset", "ichar", "idate", "idim", "idint",
                          "idnint", "ieor", "ierrno", "ifix", "imag",
                          "imagpart", "index", "int", "ior", "irand",
                          "isatty", "ishft", "ishftc", "isign",
                          "iso_c_binding", "is_iostat_end", "is_iostat_eor",
                          "itime", "kill", "kind", "lbound", "len", "len_trim",
                          "lge", "lgt", "link", "lle", "llt", "lnblnk", "loc",
                          "log", "logical", "long", "lshift", "lstat", "ltime",
                          "matmul", "max", "maxexponent", "maxloc", "maxval",
                          "mclock", "merge", "move_alloc", "min", "minexponent",
                          "minloc", "minval", "mod", "modulo", "mvbits",
                          "nearest", "new_line", "nint", "not", "or", "pack",
                          "perror", "precision", "present", "product", "radix",
                          "rand", "random_number", "random_seed", "range",
                          "real", "realpart", "rename", "repeat", "reshape",
                          "rrspacing", "rshift", "same_type_as", "scale",
                          "scan", "second", "selected_int_kind",
                          "selected_real_kind", "set_exponent", "shape",
                          "short", "sign", "signal", "sinh", "sin", "sleep",
                          "sngl", "spacing", "spread", "sqrt", "srand", "stat",
                          "sum", "symlnk", "system", "system_clock", "tan",
                          "tanh", "time", "tiny", "transfer", "transpose",
                          "trim", "ttynam", "ubound", "umask", "unlink",
                          "unpack", "verify", "xor", "zabs", "zcos", "zexp",
                          "zlog", "zsin", "zsqrt"]);

    var dataTypes =  words(["VWbool", "VWchar", "VWdouble", "VWdouble_complex",
                     "VWfloat", "VWfloat_complex", "VWfunptr", "VWint",
                     "VWint16_t", "VWint32_t", "VWint64_t", "VWint8_t",
                     "VWint_fast16_t", "VWint_fast32_t", "VWint_fast64_t",
                     "VWint_fast8_t", "VWint_least16_t", "VWint_least32_t",
                     "VWint_least64_t", "VWint_least8_t", "VWintmax_t",
                     "VWintptr_t", "VWlong", "VWlong_double",
                     "VWlong_double_complex", "VWlong_long", "VWptr",
                     "VWshort", "VWsigned_char", "VWsize_t", "character",
                     "complex", "double", "integer", "logical", "real"]);
  var isOperatorChar = /[+\-*&=<>\/\:]/;
  var litOperator = new RegExp("(\.and\.|\.or\.|\.eq\.|\.lt\.|\.le\.|\.gt\.|\.ge\.|\.ne\.|\.not\.|\.eqv\.|\.neqv\.)", "i");

  function tokenBase(stream, state) {

    if (stream.match(litOperator)){
        return 'operator';
    }

    var ch = stream.next();
    if (ch == "!") {
      stream.skipToEnd();
      return "comment";
    }
    if (ch == '"' || ch == "'") {
      state.tokenize = tokenString(ch);
      return state.tokenize(stream, state);
    }
    if (/[\[\]\(\),]/.test(ch)) {
      return null;
    }
    if (/\d/.test(ch)) {
      stream.eatWhile(/[\w\.]/);
      return "number";
    }
    if (isOperatorChar.test(ch)) {
      stream.eatWhile(isOperatorChar);
      return "operator";
    }
    stream.eatWhile(/[\w\$_]/);
    var word = stream.current().toLowerCase();

    if (keywords.hasOwnProperty(word)){
            return 'keyword';
    }
    if (builtins.hasOwnProperty(word) || dataTypes.hasOwnProperty(word)) {
            return 'builtin';
    }
    return "variable";
  }

  function tokenString(quote) {
    return function(stream, state) {
      var escaped = false, next, end = false;
      while ((next = stream.next()) != null) {
        if (next == quote && !escaped) {
            end = true;
            break;
        }
        escaped = !escaped && next == "\\";
      }
      if (end || !escaped) state.tokenize = null;
      return "string";
    };
  }

  // Interface

  return {
    startState: function() {
      return {tokenize: null};
    },

    token: function(stream, state) {
      if (stream.eatSpace()) return null;
      var style = (state.tokenize || tokenBase)(stream, state);
      if (style == "comment" || style == "meta") return style;
      return style;
    }
  };
});

CodeMirror.defineMIME("text/x-fortran", "fortran");

});
