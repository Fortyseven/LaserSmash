using System;

namespace UnityEngine.Advertisements {

  public class ShowOptions {

    public bool pause { get; set; }

    public Action<ShowResult> resultCallback { get; set; }

  }

}
