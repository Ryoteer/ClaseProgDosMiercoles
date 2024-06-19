using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdate
{
    public void ArtUpdate();
    public void ArtFixedUpdate();
    public void ArtLateUpdate();
}
